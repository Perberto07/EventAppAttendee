using EventApp.Services.NewEventService;
using EventApp.Shared.DTOs.NewEvent;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewEventController : ControllerBase
    {
        private readonly IEventServ _eventService;

        public NewEventController(IEventServ eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<NEventDto>> CreateEvent(NCreateEventDto dto)
        {
            try
            {
                var result = await _eventService.CreateEventAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
