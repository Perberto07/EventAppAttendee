using EventApp.Shared.DTOs.NewEvent;

namespace EventApp.Frontend.Services.NewEventServ
{
    public interface IClientEventServ
    {
        Task<NEventDto?> CreateEventAsync(NCreateEventDto dto);
    }
}
