using EventApp.Shared.DTOs.Location;

namespace EventApp.Frontend.Services.NewClientLocationService
{
    public interface IClientLocationService
    {
        Task<LocationDto?> CreateAsync(CreateLocationDto dto);
        Task<bool> BindSeatLayoutAsync(Guid locationId, Guid seatLayoutId);
        Task<bool> UnbindSeatLayoutAsync(Guid locationId, Guid seatLayoutId);
        Task<LocationDto?> GetLocationWithSeatLayoutsAsync(Guid locationId);
        Task<List<LocationDto>> GetAllLocationsAsync();
        Task<bool> UpdateLocationStatusAsync(Guid locationId, bool isActive);
    }
}
