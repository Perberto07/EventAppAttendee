using EventApp.Data;
using EventApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.EventAvailability
{
    public class EventAvailability : IEventAvailabilityService
    {
        private readonly DataContext _context;
        public EventAvailability(DataContext context) => _context = context;

        public async Task<bool> IsAvailableAsync(DateTime start, DateTime end, Guid locationId, Guid? excludeEventId = null)
        {
            if (start < DateTime.UtcNow)
                throw new ArgumentException("Event start time cannot be in the past.");

            if (end <= start)
                throw new ArgumentException("Event end time must be after start time.");

            var hasOverlap = await _context.Events
                .Where(e => e.Status != EventStatus.Rejected)
                .Where(e => e.LocationId == locationId) // ✅ only check events at this location
                .Where(e => excludeEventId == null || e.Id != excludeEventId.Value)
                .AnyAsync(e =>
                    e.StartDateTime < end &&   // existing starts before new ends
                    start < e.EndDateTime      // and existing ends after new starts
                );

            return !hasOverlap;
        }
    }
}
