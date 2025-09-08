using EventApp.Shared.DTOs.NewEvent;

namespace EventApp.Services.NewEventService
{
    public interface IEventServ
    {
        Task<NEventDto?> CreateEventAsync(NCreateEventDto dto);
    }
}
