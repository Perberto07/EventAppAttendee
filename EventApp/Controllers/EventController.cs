using EventApp.Data;
using EventApp.Services.EventAvailability;
using EventApp.Services.EventServices;
using EventApp.Shared.DTOs.Event;
using EventApp.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContext;

        public EventController(IEventService eventService, IHttpContextAccessor httpContext)
        {
            _eventService = eventService;
            _httpContext = httpContext;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Organizer))]
        public async Task<IActionResult> CreateEvent(CreateEventDto dto)
        {
            var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            try
            {
                var eventId = await _eventService.CreateEventAsync(dto, Guid.Parse(userId));
                return Ok(eventId);
            }
            catch (InvalidOperationException ex)
            {
                // Overlapping date/time detected
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // Invalid start/end time or missing values
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllEvents()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            var events = await _eventService.GetEventsAsync(userId, role);
            return Ok(events);
        }

        [HttpPut("{eventId}/approve")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> ApproveEvent(Guid eventId)
        {
            var success = await _eventService.ApproveEventAsync(eventId);
            return success ? Ok() : NotFound();
        }

        [HttpPut("{eventId}/reject")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> RejectEvent(Guid eventId)
        {
            var success = await _eventService.RejectEventAsync(eventId);
            return success ? Ok() : NotFound();
        }

        [HttpPut("status/{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateEventStatus(Guid id, [FromBody] UpdateEventStatusDto dto)
        {
            var success = await _eventService.UpdateEventStatusAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("check-availability")]
        [Authorize]
        public async Task<IActionResult> CheckAvailability([FromServices] IEventAvailabilityService availability,
                                                          [FromQuery] DateTime start,
                                                          [FromQuery] DateTime end)
                {
                    if (end <= start)
                        return BadRequest(new { message = "End must be after start." });

                    var ok = await availability.IsAvailableAsync(start, end);
                    return Ok(new AvailabilityDto { Available = ok });
                }

        [HttpGet("upcoming")]
        public async Task<ActionResult<List<EventDto>>> GetUpcomingEvents()
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            return Ok(events);
        }

        [HttpPut("{eventId:guid}")]
        [Authorize(Roles = $"{nameof(UserRole.Organizer)},{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> UpdateEvent(Guid eventId, UpdateEventDto dto)
        {
            var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

            if (userId is null || role is null) return Unauthorized();

            var updated = await _eventService.UpdateEventAsync(eventId, dto, Guid.Parse(userId), role);
            if (!updated) return Forbid();

            return Ok(new { message = "Event Updated Successfully" });
        }

        [HttpDelete("{eventId:guid}")]
        [Authorize(Roles = $"{nameof(UserRole.Organizer)},{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> DeleteEvent(Guid eventId)
        {
            var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

            if(userId is null || role is null) return Unauthorized();

            var delete = await _eventService.DeleteEventAsync(eventId, Guid.Parse(userId), role);

            if(!delete) return Forbid();
            return Ok(new { message = "event Deleted successfully" });
        }
    }
}
