using Application.DTO.Application.User;
using Core.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IUserService 
    {
        Task<UseCaseResponse<UserDto>> GetById(Guid userId);
        Task<UseCaseResponse<AuthenticateResponseDTO>> Authenticate(AuthenticateRequestDTO model);
        Task<UseCaseResponse<RegisterAdminResponseDTO>> RegisterAdmin(RegisterAdminRequestDTO model);
        Task<UseCaseResponse<RegisterDeliveryPersonResponseDTO>> RegisterDeliveryPerson(RegisterDeliveryPersonRequestDTO model);
        Task<UseCaseResponse<UploadPhotoResponseDTO>> UploadPhoto(IFormFile formFile);
    }
}
