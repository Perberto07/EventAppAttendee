using EventApp.Shared.DTOs.Common;
using EventApp.Shared.DTOs.Seat;


namespace EventApp.Frontend.Services.SeatLayoutClient
{
    public interface ISeatLayoutClient
    {
        Task<IEnumerable<SeatLayoutDto>> GetAllAsync();

        Task<PagedResult<SeatDto>> GetSeatsPagedAsync(Guid layoutId, PaginationParams pagination);
        Task<SeatLayoutDto?> GetByIdAsync(Guid id);
        Task<Guid?> CreateAsync(CreateSeatLayoutDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
