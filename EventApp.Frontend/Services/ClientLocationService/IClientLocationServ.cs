using EventApp.Shared.DTOs.Location;
using EventApp.Shared.DTOs.Seat;

namespace EventApp.Frontend.Services.ClientLocationService
{
    public interface IClientLocationServ
    {
        Task<List<LocationDto>> GetAllAsync();
        Task<LocationDto?> GetByIdAsync(Guid id);
        Task<LocationDto> CreateAsync(CreateLocationDto dto);
        Task<LocationDto?> UpdateAsync(UpdateLocationDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<SeatLayoutDto>> GetSeatLayoutsByLocationAsync(Guid locationId);
        Task<bool> BindSeatLayoutsAsync(Guid locationId, List<Guid> seatLayoutIds);
        Task<bool> UnbindSeatLayoutsAsync(Guid locationId, List<Guid> seatLayoutIds);
    }
}
