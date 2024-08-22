using Application.DTO.Application.Motorcycle;
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
    public class MotorcycleController(IMotorcycleService motorcycleService) : Controller
    {
        private readonly IMotorcycleService motorcycleService = motorcycleService;

        [HttpGet("id")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetAsync(Guid id)
        => Ok(await this.motorcycleService.GetAsync(id));

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetListAsync([FromQuery] MotorcycleGetRequestDTO request)
        => Ok(await this.motorcycleService.GetListAsync(request));


        [HttpPost]
        [Authorize(Roles = Constants.ROLE_NAME_ADMIN)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Forbidden, Type = typeof(string))]
        public async Task<IActionResult> PostAsync([FromBody] MotorcycleAddRequestDTO request)
        => Ok(await this.motorcycleService.AddAsync(request));


        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] MotorcycleEditRequestDTO request)
        => Ok(await this.motorcycleService.EditAsync(id, request));



        [HttpDelete("id")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        => Ok(await this.motorcycleService.DeleteAsync(id));
    }
}
