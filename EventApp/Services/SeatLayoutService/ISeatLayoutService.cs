using EventApp.Shared.DTOs.Common;
using EventApp.Shared.DTOs.Seat;

namespace EventApp.Services.SeatLayoutService
{
    public interface ISeatLayoutService
    {
        Task<IEnumerable<SeatLayoutDto>> GetAllAsync();
        Task<SeatLayoutDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateSeatLayoutDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<PagedResult<SeatDto>> GetSeatsPagedAsync(Guid layoutId, PaginationParams pagination);

    }
}
