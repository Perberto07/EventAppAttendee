//using EventApp.Data;
//using EventApp.Services.EventTransactionServ;
//using EventApp.Services.PhilippineTimeService;
//using EventApp.Shared.DTOs.Event;
//using EventApp.Shared.DTOs.EventTransactionDto;
//using EventApp.Shared.Enums;
//using EventApp.Shared.Models;
//using Microsoft.EntityFrameworkCore;

//namespace EventApp.Services.EventServices
//{
//    public class EventService : IEventService
//    {
//        private readonly DataContext _context;
//        private readonly ITransactionServ _transaction;
//        private readonly IPhTimeServ _philTimeServ;

//        public EventService(DataContext context, ITransactionServ transaction, IPhTimeServ philTimeServ)
//        {
//            _context = context;
//            _transaction = transaction;
//            _philTimeServ = philTimeServ;
//        }
//        public async Task<Guid> CreateEventAsync(CreateEventDto dto, Guid organizerId)
//        {
//            // ✅ Check availability within the same location
//            if (!dto.LocationId.HasValue)
//            {
//                throw new InvalidOperationException("Location must be selected.");
//            }
//            var available = await IsAvailableAsync(dto.StartDateTime, dto.EndDateTime, dto.LocationId.Value);

//            if (!available)
//                throw new InvalidOperationException("The selected time slot is not available for this location.");

//            var seatLayout = await _context.SeatLayouts
//                .Include(sl => sl.Seats)
//                .FirstOrDefaultAsync(sl => sl.Id == dto.SeatLayoutId);

//            if (seatLayout == null)
//                throw new ArgumentException("Invalid Seat Layout selected.");

//            var newEvent = new Event
//            {
//                Title = dto.Title,
//                Description = dto.Description,
//                StartDateTime = dto.StartDateTime,
//                EndDateTime = dto.EndDateTime,
//                OrganizerId = organizerId,
//                Status = dto.Status,
//                Price = dto.Price,
//                LocationId = dto.LocationId, // ✅ Add Location
//                SeatLayoutId = dto.SeatLayoutId,
//            };

//            newEvent.EventSeats = seatLayout.Seats
//                .Select(s => new EventSeat
//                {
//                    SeatId = s.Id,
//                    IsBooked = false
//                })
//                .ToList();


           

//            _context.Events.Add(newEvent);
//            await _context.SaveChangesAsync();

//            var transactionDto = new CreateEventTransactionDto
//            {
//                EventId = newEvent.Id,
//                UserId = organizerId, // assuming organizer is the first transaction owner
//                Price = seatLayout.Price
//            };

//            await _transaction.CreateTransactionAsync(transactionDto);

//            return newEvent.Id;
//        }


//        public async Task<List<EventDto>> GetEventsAsync(Guid userId, string role)
//        {
//            IQueryable<Event> query = _context.Events;

//            if (role == UserRole.Organizer.ToString())
//            {
//                query = query.Where(e => e.OrganizerId == userId);
//            }

//            return await query
//                .Select(e => new EventDto
//                {
//                    Id = e.Id,
//                    Title = e.Title,
//                    Description = e.Description,
//                    StartDateTime = e.StartDateTime,
//                    EndDateTime = e.EndDateTime,
//                    Location = e.Location.Name,
//                    OrganizerId = e.OrganizerId,
//                    Status = e.Status,
//                    Price = e.Price
//                })
//                .ToListAsync();
//        }

//        public async Task<bool> ApproveEventAsync(Guid eventId)
//        {
//            var ev = await _context.Events.FindAsync(eventId);
//            if (ev == null) return false;

//            ev.Status = EventStatus.Approved;
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> RejectEventAsync(Guid eventId)
//        {
//            var ev = await _context.Events.FindAsync(eventId);
//            if (ev == null) return false;

//            ev.Status = EventStatus.Rejected;
//            await _context.SaveChangesAsync();

//            await _transaction.UpdateTransactionStatusByEventAsync(eventId, ev.Status.ToString());
//            return true;
//        }

//        public async Task<bool> UpdateEventStatusAsync(Guid eventId, UpdateEventStatusDto dto)
//        {
//            var ev = await _context.Events.FindAsync(eventId);
//            if (ev == null) return false;

//            ev.Status = dto.Status;
//            await _context.SaveChangesAsync();

//            await _transaction.UpdateTransactionStatusByEventAsync(eventId, ev.Status.ToString());
            
//            return true;
//        }

//        public Task<bool> AnyOverlapAsync(DateTime start, DateTime end, Guid locationId, Guid? excludeEventId = null)
//        {
//            return _context.Events
//                .Where(e => e.Status != EventStatus.Rejected)
//                .Where(e => e.LocationId == locationId) // ✅ Same location only
//                .Where(e => excludeEventId == null || e.Id != excludeEventId.Value)
//                .AnyAsync(e =>
//                    e.StartDateTime < end &&
//                    start < e.EndDateTime
//                );
//        }

//        public Task<bool> IsAvailableAsync(DateTime start, DateTime end, Guid locationId, Guid? excludeEventId = null)
//        {
//            if (start < DateTime.UtcNow)
//                throw new ArgumentException("Event start time cannot be in the past.");

//            if (end <= start)
//                throw new ArgumentException("Event end time must be after start time.");

//            return _context.Events
//                .Where(e => e.Status != EventStatus.Rejected)
//                .Where(e => e.LocationId == locationId) // ✅ Only check same location
//                .Where(e => excludeEventId == null || e.Id != excludeEventId.Value)
//                .AnyAsync(e =>
//                    e.StartDateTime < end &&   // existing starts before new ends
//                    start < e.EndDateTime      // and existing ends after new starts
//                )
//                .ContinueWith(t => !t.Result); // return true if none overlap
//        }


//        public async Task<List<EventDto>> GetUpcomingEventsAsync()
//        {
//            var now = _philTimeServ.ToPhilippineTime(DateTime.UtcNow);

//            return await _context.Events
//                .Where(e => e.StartDateTime >= now && e.Status == EventStatus.Approved)
//                .OrderBy(e => e.StartDateTime)
//                .Select(e => new EventDto
//                {
//                    Id = e.Id,
//                    Title = e.Title,
//                    Description = e.Description,
//                    StartDateTime = e.StartDateTime,
//                    EndDateTime = e.EndDateTime,
//                    Location = e.Location.Name,
//                    OrganizerId = e.OrganizerId,
//                    Status = e.Status,
//                    Price = e.Price
//                })
//                .ToListAsync();
//        }

//        public async Task<bool> UpdateEventAsync(Guid eventId, UpdateEventDto dto, Guid userId, string role)
//        {
//            var evnt = await _context.Events.FindAsync(eventId);
//            if (evnt == null) return false;

//            // Only the organizer who created the event can update
//            if (role == "Organizer" && evnt.OrganizerId != userId)
//                return false;

//            // Validate end time
//            if (dto.EndDateTime <= dto.StartDateTime)
//                throw new ArgumentException("Event end time must be after start time.");

//            // Check availability for the new time slot, excluding the current event
//            bool available = await IsAvailableAsync(dto.StartDateTime, dto.EndDateTime, evnt.LocationId.Value, eventId);
//            if (!available)
//                throw new InvalidOperationException("The selected time slot is not available for this location.");

//            // Update basic fields
//            evnt.Title = dto.Title;
//            evnt.Description = dto.Description;
//            evnt.StartDateTime = dto.StartDateTime;
//            evnt.EndDateTime = dto.EndDateTime;
//            evnt.Price = dto.Price;

//            bool statusChanged = false;

//            // Organizer cannot change status
//            if (role == UserRole.Organizer.ToString())
//            {
//                dto.Status = evnt.Status; // ignore incoming status change
//            }

//            // Admin can update status
//            if (role == UserRole.Admin.ToString() && evnt.Status != dto.Status)
//            {
//                evnt.Status = dto.Status;
//                statusChanged = true;
//            }

//            await _context.SaveChangesAsync();

//            // ✅ Cascade to related transactions if status changed
//            if (statusChanged)
//            {
//                await _transaction.UpdateTransactionStatusByEventAsync(eventId, evnt.Status.ToString());
//            }

//            return true;
//        }

//        public async Task<bool> DeleteEventAsync(Guid eventId, Guid userId, string role)
//        {
//            var evnt = await _context.Events
//                .Include(e => e.EventSeats) // Include EventSeats to delete them too
//                .ThenInclude(s => s.Tickets)
//                .FirstOrDefaultAsync(e => e.Id == eventId);

//            if (evnt == null) return false;

//            if(role == "Organizer" && evnt.OrganizerId != userId)
//                return false;

//            _context.EventSeats.RemoveRange(evnt.EventSeats);
//            _context.Events.Remove(evnt);
//            await _context.SaveChangesAsync();

//            return true;

//        }
//    }
//}
