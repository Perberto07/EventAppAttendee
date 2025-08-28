using EventApp.Data;
using EventApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.EventAvailability
{
    public class EventAvailability : IEventAvailabilityService
    {
        private readonly DataContext _context;
        public EventAvailability(DataContext context) => _context = context;

        public Task<bool> IsAvailableAsync(DateTime start, DateTime end, Guid? excludeEventId = null)
        {
            // available = NOT any overlap
            return _context.Events
                .Where(e => e.Status != EventStatus.Rejected)
                .Where(e => excludeEventId == null || e.Id != excludeEventId.Value)
                .AnyAsync(e =>
                    e.StartDateTime < end &&   // existing starts before new ends
                    start < e.EndDateTime      // and existing ends after new starts
                ).ContinueWith(t => !t.Result);
        }

    }
}
