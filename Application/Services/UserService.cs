using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;

using Application.Core.Extensions;
using Application.DTO.Application.User;
using Application.DTO.Configuration;
using Application.Interfaces;
using Application.Validations;
using Core.Entities;
using Core;
using Core.Response;
using Infrastructure.UnitOfWork;

namespace Application.Services
{
    public class UserService(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        UserManager<UserModel> userManager,
        RoleManager<RoleModel> roleManager,
        SignInManager<UserModel> signInManager,
        IOptions<JwtOptionsSettings> jwtOptionsSettings) : IUserService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly SignInManager<UserModel> signInManager = signInManager;
        private readonly UserManager<UserModel> userManager = userManager;
        private readonly RoleManager<RoleModel> roleManager = roleManager;
        private readonly IOptions<JwtOptionsSettings> jwtOptionsSettings = jwtOptionsSettings;
        private readonly RegisterAdminRequestValidation adminRequestValidation = new();
        private readonly RegisterDeliveryPersonRequestValidation deliveryPersonRequestValidation = new();
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IMapper mapper = mapper;

        public async Task<UseCaseResponse<AuthenticateResponseDTO>> Authenticate(AuthenticateRequestDTO model)
        {
            UseCaseResponse<AuthenticateResponseDTO> response = new();
            UserModel? user = (await this.unitOfWork.User.Get(x => x.UserName.Equals(model.UserName))).FirstOrDefault();

            if (user is null)
                return response.NotFound($"User not found with username: {model.UserName}");

            SignInResult checkPassword = await this.signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!checkPassword.Succeeded)
                return response.BadRequest($"Password is not valid");

            AuthenticateResponseDTO dto = this.mapper.Map<AuthenticateResponseDTO>(user);
            dto.Token = await GenerateJwtToken(user);

            return new UseCaseResponse<AuthenticateResponseDTO>().Ok(dto);
        }

        public async Task<UseCaseResponse<UserDto>> GetById(Guid userId)
        {
            UserModel user = await this.unitOfWork.User.Get(userId);

            return new UseCaseResponse<UserDto>().Ok(this.mapper.Map<UserDto>(user));
        }

        public async Task<UseCaseResponse<RegisterAdminResponseDTO>> RegisterAdmin(RegisterAdminRequestDTO model)
        {
            UseCaseResponse<RegisterAdminResponseDTO> useCaseResponse = new();
            ValidationResult validationResult = await adminRequestValidation.ValidateAsync(model);

            if (!validationResult.IsValid)
                return useCaseResponse.BadRequest($"Error occurred in the request: {string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))}");

            UserModel check = (await this.unitOfWork.User.Get(x => x.UserName.Equals(model.UserName))).FirstOrDefault();

            if (check is not null)
                useCaseResponse.BadRequest($"User {model.UserName} already exists.");

            UserModel user = this.mapper.Map<UserModel>(model);

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return useCaseResponse.BadRequest($"Error occurred while creating a new user: {string.Join(", ", result.Errors.Select(x => x.Description).ToList())}");

            await this.userManager.AddToRoleAsync(user, Constants.ROLE_NAME_ADMIN);

            RegisterAdminResponseDTO response = this.mapper.Map<RegisterAdminResponseDTO>(user);

            response.Token = await GenerateJwtToken(user);

            return useCaseResponse.Ok(response);
        }

        public async Task<UseCaseResponse<RegisterDeliveryPersonResponseDTO>> RegisterDeliveryPerson(RegisterDeliveryPersonRequestDTO model)
        {
            UseCaseResponse<RegisterDeliveryPersonResponseDTO> useCaseResponse = new();
            ValidationResult validationResult = await deliveryPersonRequestValidation.ValidateAsync(model);

            if (!validationResult.IsValid)
                return useCaseResponse.BadRequest($"Error occurred in the request: {string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))}");

            if ((await this.unitOfWork.User.Get(x => x.UserName.Equals(model.UserName))).FirstOrDefault() is not null)
                return useCaseResponse.BadRequest($"User {model.UserName} already exists.");

            if ((await this.unitOfWork.User.Get(x => model.CorporateNumber.Contains(x.CorporateNumber))).FirstOrDefault() is not null)
                return useCaseResponse.BadRequest($"Corporate Number {model.CorporateNumber} already exists");

            if ((await this.unitOfWork.User.Get(x => model.DriverLicense.Contains(x.DriverLicense))).FirstOrDefault() is not null)
                return useCaseResponse.BadRequest($"Driver License {model.DriverLicense} already exists");

            UserModel user = this.mapper.Map<UserModel>(model);

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return useCaseResponse.BadRequest($"Error occurred while creating a new user: {string.Join(", ", result.Errors.Select(x => x.Description).ToList())}");
            
            await this.userManager.AddToRoleAsync(user, Constants.ROLE_NAME_DELIVERY_PERSON);

            RegisterDeliveryPersonResponseDTO response = this.mapper.Map<RegisterDeliveryPersonResponseDTO>(user);

            response.Token = await GenerateJwtToken(user);

            return useCaseResponse.Ok(response);
        }

        public async Task<UseCaseResponse<UploadPhotoResponseDTO>> UploadPhoto(IFormFile formFile)
        {
            UseCaseResponse<UploadPhotoResponseDTO> useCaseResponse = new();
            List<string> permittedExtensions = new() { ".jpg", ".jpeg", ".png" };

            if (formFile == null || formFile.Length == 0)
                return useCaseResponse.BadRequest("No file provided.");

            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return useCaseResponse.BadRequest("Only PNG and JPG files are allowed.");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await formFile.CopyToAsync(stream);

            UploadPhotoResponseDTO entity = new() 
            { 
                FileName = fileName 
            };

            UserModel user = await this.unitOfWork.User.Get(this.httpContextAccessor.HttpContext.User.Id());
            user.Photo = fileName;

            this.unitOfWork.User.Update(user);
            await this.unitOfWork.CommitAsync();

            return useCaseResponse.Ok(entity);
        }

        private async Task<string> GenerateJwtToken(UserModel user)
        {
            JwtSecurityTokenHandler? tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken? token = await Task.Run(async () =>
            {
                var key = Encoding.ASCII.GetBytes(this.jwtOptionsSettings.Value.Secret);
                DateTime expiresAt = DateTime.UtcNow + TimeSpan.FromHours(10);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                List<string> roles = (await this.userManager.GetRolesAsync(user)).ToList();

                if (roles.Any())
                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiresAt,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
