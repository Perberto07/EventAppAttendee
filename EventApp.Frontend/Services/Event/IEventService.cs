using EventApp.Shared.DTOs.Event;
using EventApp.Shared.Enums;

namespace EventApp.Frontend.Services.Event
{
    public interface IEventService
    {
        Task<Guid?> CreateEventAsync(CreateEventDto dto);
        Task<List<EventDto>> GetAllEventsAsync();
        Task<bool> ApproveEventAsync(Guid eventId);
        Task<bool> RejectEventAsync(Guid eventId);

        Task<bool> UpdateEventStatusAsync(Guid eventId, EventStatus newStatus);
        Task<AvailabilityDto?> CheckAsync(DateTime start, DateTime end);
        Task<List<EventDto>> GetUpcomingEventsAsync();
        Task<bool> UpdateEventAsync(Guid eventId, UpdateEventDto dto);
        Task<bool> DeleteEventAsync(Guid eventId);

    }
}
