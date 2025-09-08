using EventApp.Shared.DTOs.Event;

namespace EventApp.Services.EventServices
{
    public interface IEventService
    {
        Task<Guid> CreateEventAsync(CreateEventDto dto, Guid organizerId);
        Task<List<EventDto>> GetEventsAsync(Guid userId, string role);
        Task<bool> ApproveEventAsync(Guid eventId);
        Task<bool> RejectEventAsync(Guid eventId);
        Task<bool> UpdateEventStatusAsync(Guid eventId, UpdateEventStatusDto dto);
        Task<bool> AnyOverlapAsync(DateTime start, DateTime end, Guid locationId, Guid? excludeEventId = null);
        Task<bool> IsAvailableAsync(DateTime start, DateTime end, Guid locationId, Guid? excludeEventId = null);
        Task<List<EventDto>> GetUpcomingEventsAsync();

        Task<bool> UpdateEventAsync(Guid eventId, UpdateEventDto dto, Guid userId, string role);
        Task<bool> DeleteEventAsync(Guid eventId, Guid userId, string role);
    } 
}
        