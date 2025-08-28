using EventApp.Data;
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
        public async Task<SeatSummaryDto> GetSeatsByEventAsync(Guid eventId)
        {
            var ev = await _context.Events
                .Where(e => e.Id == eventId)
                .Select(e => new { e.Price })
                .FirstOrDefaultAsync();

            if (ev == null)
                throw new Exception("Event not found");

            var seats = await _context.EventSeats
                .Include(es => es.Seat)
                .Where(es => es.EventId == eventId)
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

            var bookedCount = seats.Count(s => s.IsBooked);

            return new SeatSummaryDto
            {
                TotalSeats = seats.Count,
                AvailableSeats = seats.Count - bookedCount,
                BookedSeats = bookedCount,
                TicketPrice = ev.Price,
                TotalRevenue = bookedCount * ev.Price, // 💰 profit from sales
                Seats = seats
            };
        }

    }
}
