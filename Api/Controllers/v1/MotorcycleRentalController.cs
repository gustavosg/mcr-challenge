
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Application.DTO.Application.MotorcycleRental;
using Application.Interfaces;

namespace Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MotorcycleRentalController(IMotorcycleRentalService motorcycleRentalService) : Controller
    {
        private readonly IMotorcycleRentalService motorcycleRentalService = motorcycleRentalService;

        [HttpGet("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetAsync(Guid id)
            => Ok(await this.motorcycleRentalService.GetAsync(id));

        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetListAsync([FromQuery] MotorcycleRentalGetRequestDTO request)
            => Ok(await this.motorcycleRentalService.GetListAsync(request));

        [HttpGet("simulate-price")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public async Task<IActionResult> SimulatePrice(Guid id, DateOnly dateReturn)
        => Ok(await this.motorcycleRentalService.SimulateMotorcycleReturn(id, dateReturn));

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public async Task<IActionResult> PostAsync([FromBody] MotorcycleRentalAddRequestDTO request)
            => Ok(await this.motorcycleRentalService.AddAsync(request));
    }
}
