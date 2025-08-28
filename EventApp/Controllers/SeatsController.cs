using EventApp.Services.SeatService;
using EventApp.Shared.DTOs.Seat;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<List<EventSeatDto>>> GetSeatsByEvent(Guid eventId)
        {
            var seats = await _seatService.GetSeatsByEventAsync(eventId);
            return Ok(seats);
        }

    }

}
