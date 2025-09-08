using EventApp.Services.NewLocationService;
using EventApp.Shared.DTOs.Location;
using EventApp.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LayoutLocationController : ControllerBase
    {
        private readonly ILocationServ _service;

        public LayoutLocationController(ILocationServ service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<ActionResult<LocationDto>> CreateLocation(CreateLocationDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return created;
        }
        [HttpPost("bind")]
        public async Task<IActionResult> Bind([FromBody] BindSeatLayoutDto dto)
        {
            var success = await _service.BindSeatLayoutAsync(dto.LocationId, dto.SeatLayoutId);
            if (!success) return BadRequest("Binding failed. Either Location or SeatLayout not found or inactive.");
            return Ok("SeatLayout successfully bound to Location.");
        }

        [HttpPost("unbind")]
        public async Task<IActionResult> Unbind([FromBody] BindSeatLayoutDto dto)
        {
            var success = await _service.UnbindSeatLayoutAsync(dto.LocationId, dto.SeatLayoutId);
            if (!success) return BadRequest("Unbinding failed. Location or SeatLayout not found.");
            return Ok("SeatLayout successfully unbound from Location.");
        }

        [HttpGet("{locationId}")]
        public async Task<IActionResult> Get(Guid locationId)
        {
            var result = await _service.GetLocationWithSeatLayoutsAsync(locationId);
            if (result == null) return NotFound("Location not found.");
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationDto>>> GetAllLocations()
        {
            var locations = await _service.GetAllLocationsAsync();
            return Ok(locations);
        }

        [HttpPatch("{locationId}/status")]
        public async Task<IActionResult> UpdateLocationStatus(Guid locationId, [FromQuery] bool isActive)
        {
            var success = await _service.UpdateLocationStatusAsync(locationId, isActive);
            if (!success) return NotFound();

            return NoContent();
        }

    }
}
