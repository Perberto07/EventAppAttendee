using EventApp.Shared.DTOs.Location;

namespace EventApp.Services.NewLocationService
{
    public interface ILocationServ
    {
        Task<bool> BindSeatLayoutAsync(Guid locationId, Guid seatLayoutId);
        Task<LocationDto> CreateAsync(CreateLocationDto dto);
        Task<bool> UnbindSeatLayoutAsync(Guid locationId, Guid seatLayoutId);
        Task<LocationDto?> GetLocationWithSeatLayoutsAsync(Guid locationId);
        Task<List<LocationDto>> GetAllLocationsAsync();
        Task<bool> UpdateLocationStatusAsync(Guid locationId, bool isActive);
    }
}
