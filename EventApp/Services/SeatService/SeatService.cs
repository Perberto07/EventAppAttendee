using EventApp.Data;
using EventApp.Shared.DTOs.Common;
using EventApp.Shared.DTOs.Seat;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.SeatService
{
    public class SeatService : ISeatService
    {
        private readonly DataContext _context;

        public SeatService(DataContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<SeatSummaryDto>> GetSeatsByEventAsync(Guid eventId, PaginationParams pagination)
        {
            // Get event price
            var ev = await _context.Events
                .Where(e => e.Id == eventId)
                .Select(e => new { e.Price })
                .FirstOrDefaultAsync();

            if (ev == null)
                throw new Exception("Event not found");

            // Distinct row numbers for pagination
            var distinctRows = await _context.EventSeats
                .Where(es => es.EventId == eventId)
                .Select(es => es.Seat.RowNumber)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            int rowsPerPage = pagination.PageSize;
            var rowsForPage = distinctRows
                .Skip((pagination.PageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToList();

            // Fetch only seats in the current page
            var pagedSeats = await _context.EventSeats
                .Include(es => es.Seat)
                .Where(es => es.EventId == eventId && rowsForPage.Contains(es.Seat.RowNumber))
                .OrderBy(es => es.Seat.RowNumber)
                .ThenBy(es => es.Seat.ColumnNumber)
                .Select(es => new EventSeatDto
                {
                    EventSeatId = es.Id,
                    SeatNumber = es.Seat.SeatNumber,
                    RowNumber = es.Seat.RowNumber,
                    ColumnNumber = es.Seat.ColumnNumber,
                    IsBooked = es.IsBooked
                })
                .ToListAsync();

            // Totals
            var totalSeats = await _context.EventSeats.CountAsync(es => es.EventId == eventId);
            var bookedCount = await _context.EventSeats.CountAsync(es => es.EventId == eventId && es.IsBooked);

            var seatSummary = new SeatSummaryDto
            {
                TotalSeats = totalSeats,
                AvailableSeats = totalSeats - bookedCount,
                BookedSeats = bookedCount,
                TicketPrice = ev.Price,
                TotalRevenue = bookedCount * ev.Price,
                Seats = pagedSeats
            };
            var StatusSummary = new SeatSummaryDto
            {
                TotalSeats = totalSeats,
                AvailableSeats = totalSeats - bookedCount,
                BookedSeats = bookedCount,
                TicketPrice = ev.Price,
                TotalRevenue = bookedCount * ev.Price,
            };

            return new PagedResult<SeatSummaryDto>
            {
                Items = new List<SeatSummaryDto> { seatSummary },
                TotalCount = distinctRows.Count, // total rows
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)distinctRows.Count / rowsPerPage),
                Summary = StatusSummary,
            };
        }

    }
}
