using EventApp.Data;
using EventApp.Shared.DTOs.Event;
using EventApp.Shared.Enums;
using EventApp.Shared.Models;

using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.EventServices
{
    public class EventService : IEventService
    {
        private readonly DataContext _context;

        public EventService(DataContext context)
        {
            _context = context;
        }
        public async Task<Guid> CreateEventAsync(CreateEventDto dto, Guid organizerId)
        {
            if (dto.EndDateTime <= dto.StartDateTime)
                throw new ArgumentException("Event end time must be after start time.");

            var seatLayout = await _context.SeatLayouts
                .Include(sl => sl.Seats)
                .FirstOrDefaultAsync(sl => sl.Id == dto.SeatLayoutId);

            if (seatLayout == null)
                throw new ArgumentException("Invalid Seat Layout selected.");

            var newEvent = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                OrganizerId = organizerId,
                Status = dto.Status,
                Price = dto.Price,
                SeatLayoutId = dto.SeatLayoutId
            };

            // ✅ Create EventSeat entries for this event
            newEvent.EventSeats = seatLayout.Seats
                .Select(s => new EventSeat
                {
                    SeatId = s.Id,
                    IsBooked = false
                })
                .ToList();

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task<List<EventDto>> GetEventsAsync(Guid userId, string role)
        {
            IQueryable<Event> query = _context.Events;

            if (role == UserRole.Organizer.ToString())
            {
                query = query.Where(e => e.OrganizerId == userId);
            }

            return await query
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    Location = e.Location,
                    OrganizerId = e.OrganizerId,
                    Status = e.Status,
                    Price = e.Price
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveEventAsync(Guid eventId)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return false;

            ev.Status = EventStatus.Approved;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectEventAsync(Guid eventId)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return false;

            ev.Status = EventStatus.Rejected;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEventStatusAsync(Guid eventId, UpdateEventStatusDto dto)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return false;

            ev.Status = dto.Status;
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> AnyOverlapAsync(DateTime start, DateTime end, Guid? excludeEventId = null)
        {
            return _context.Events
                .Where(e => e.Status != EventStatus.Rejected) // optional: ignore rejected
                .Where(e => excludeEventId == null || e.Id != excludeEventId.Value)
                .AnyAsync(e =>
                    e.StartDateTime < end &&   // existing starts before new ends
                    start < e.EndDateTime      // and existing ends after new starts
                );
        }

        public Task<bool> IsAvailableAsync(DateTime start, DateTime end, Guid? excludeEventId = null)
        {
            return _context.Events
                .Where(e => e.Status != EventStatus.Rejected)
                .Where(e => excludeEventId == null || e.Id != excludeEventId.Value)
                .AnyAsync(e =>
                    e.StartDateTime < end &&   // existing starts before new ends
                    start < e.EndDateTime      // and existing ends after new starts
                ).ContinueWith(t => !t.Result);
        }

        public async Task<List<EventDto>> GetUpcomingEventsAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Events
                .Where(e => e.StartDateTime >= now && e.Status == EventStatus.Approved)
                .OrderBy(e => e.StartDateTime)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    Location = e.Location,
                    OrganizerId = e.OrganizerId,
                    Status = e.Status,
                    Price = e.Price
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateEventAsync(Guid eventId, UpdateEventDto dto, Guid userId, string role)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null) return false;

            if (role == "Organizer" && evnt.OrganizerId != userId)
                return false;

            if(dto.EndDateTime <= dto.StartDateTime)
                throw new ArgumentException("event end time must be after start time.");
            
            if (role == UserRole.Organizer.ToString())
            {
                dto.Status = evnt.Status; // ignore incoming status change
            }

            evnt.Title = dto.Title;
            evnt.Description = dto.Description;
            evnt.StartDateTime = dto.StartDateTime;
            evnt.EndDateTime = dto.EndDateTime;
            evnt.Price = dto.Price;

            if (role == UserRole.Admin.ToString())
            {
                evnt.Status = dto.Status;
            }

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteEventAsync(Guid eventId, Guid userId, string role)
        {
            var evnt = await _context.Events
                .Include(e => e.EventSeats) // Include EventSeats to delete them too
                .ThenInclude(s => s.Tickets)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (evnt == null) return false;

            if(role == "Organizer" && evnt.OrganizerId != userId)
                return false;

            _context.EventSeats.RemoveRange(evnt.EventSeats);
            _context.Events.Remove(evnt);
            await _context.SaveChangesAsync();

            return true;

        }
    }
}
