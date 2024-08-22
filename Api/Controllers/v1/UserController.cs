using Application.DTO.Application.User;
using Application.Interfaces;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService userService = userService;


        [HttpPost("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequestDTO model)
            => Ok(await userService.Authenticate(model));

        [HttpPost("register-admin")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterAdminRequestDTO model)
            => Ok(await userService.RegisterAdmin(model));

        [HttpPost("register-delivery-person")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> RegisterDeliveryPerson([FromBody] RegisterDeliveryPersonRequestDTO model)
            => Ok(await userService.RegisterDeliveryPerson(model));

        [HttpPost("upload-photo")]
        [Authorize(Roles = Constants.ROLE_NAME_DELIVERY_PERSON)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UploadPhoto(IFormFile formFile)
         => Ok(await userService.UploadPhoto(formFile));
    }
}
