using EventApp.Shared.DTOs.Common;
using EventApp.Shared.DTOs.Seat;

namespace EventApp.Frontend.Services.EventSeatService
{
    public interface IEventSeatService
    {
        Task<PagedResult<EventSeatDto>> GetEventSeatsByEventAsync(Guid eventId, PaginationParams pagination);
    }
}
