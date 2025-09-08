using EventApp.Shared.DTOs.Common;
using EventApp.Shared.DTOs.Seat;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace EventApp.Services.SeatService
{
    public interface ISeatService
    {
        Task<PagedResult<SeatSummaryDto>> GetSeatsByEventAsync(Guid eventId, PaginationParams pagination);
    }
}
