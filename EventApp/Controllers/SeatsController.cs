//using EventApp.Services.SeatService;
//using EventApp.Shared.DTOs.Common;
//using EventApp.Shared.DTOs.Seat;
//using Microsoft.AspNetCore.Mvc;

//namespace EventApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SeatsController : ControllerBase
//    {
//        private readonly ISeatService _seatService;

//        public SeatsController(ISeatService seatService)
//        {
//            _seatService = seatService;
//        }

//        [HttpGet("event/{eventId}")]
//        public async Task<ActionResult<PagedResult<EventSeatDto>>> GetSeatsByEvent(
//            Guid eventId,
//            [FromQuery] int pageNumber = 1,
//            [FromQuery] int pageSize = 5)
//        {
//            var pagination = new PaginationParams
//            {
//                PageNumber = pageNumber,
//                PageSize = pageSize
//            };

//            // Call the service
//            var seatSummaryPaged = await _seatService.GetSeatsByEventAsync(eventId, pagination);

//            if (seatSummaryPaged.Items.Count() == 0)
//                return NotFound();

//            var seatSummary = seatSummaryPaged.Items.First(); // Only one summary per page

//            // Map to PagedResult<EventSeatDto> for frontend
//            var result = new PagedResult<EventSeatDto>
//            {
//                Items = seatSummary.Seats,
//                TotalCount = seatSummary.TotalSeats,       // total seats in event
//                PageNumber = pagination.PageNumber,
//                PageSize = seatSummary.Seats.Count,        // seats in this page
//                TotalPages = seatSummaryPaged.TotalPages,
//                Summary = seatSummaryPaged.Summary
//            };

//            return Ok(result);
//        }

//    }

//}

