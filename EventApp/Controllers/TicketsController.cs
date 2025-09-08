using EventApp.Services.TicketService;
using EventApp.Shared.DTOs.DtoTicket;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketMonitorService _ticketService;

        public TicketsController(ITicketMonitorService ticketMonitorService) 
        {
            _ticketService = ticketMonitorService;
        }

        [HttpGet("ticket/{eventId}")]
        public async Task<ActionResult<List<TicketDto>>> GetSeatsByEvent(Guid eventId)
        {
            var tickets = await _ticketService.GetTicketByEventAsync(eventId);
            return Ok(tickets);
        }
    }
}
