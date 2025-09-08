//using EventApp.Data;
//using EventApp.Shared.DTOs.DtoTicket;
//using Microsoft.EntityFrameworkCore;

//namespace EventApp.Services.TicketService
//{
//    public class TicketMonitorService : ITicketMonitorService
//    {
//        private readonly DataContext _db;

//        public TicketMonitorService(DataContext db)
//        {
//            _db = db;
//        }

//        public async Task<List<TicketDto>> GetTicketByEventAsync(Guid eventId)
//        {
//            return await _db.Tickets
//                .Where(t => t.EventId == eventId)
//                .Include(t => t.Attendee)
//                .Include(t => t.EventSeat)
//                .Select(t => new TicketDto 
//                { 
//                    Id = t.Id,
//                    EventTitle = t.Event.Title,
//                    PurchaseDate = t.PurchaseDate,
//                    attendeeName = t.Attendee.Username,
//                    Price = t.Event.Price,
//                    Location = t.Event.Location.Name,
//                    SeatNumber = t.EventSeat.Seat.SeatNumber,
//                    AttendeeEmail = t.Attendee.Email
                    
//                })
//                .ToListAsync();

//        }
//    }
//}
