using EventApp.Shared.DTOs.Seat;
using EventApp.Shared.Models;

namespace EventApp.Services.SeatLayoutService
{
    public interface ISeatLayoutService
    {
        Task<IEnumerable<SeatLayoutDto>> GetAllAsync();
        Task<SeatLayoutDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateSeatLayoutDto dto);
        Task<bool> DeleteAsync(Guid id);

    }
}
