using EventApp.Shared.DTOs.Location;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.NewClientLocationService
{
    public class ClientLocationService : IClientLocationService
    {
        private readonly HttpClient _http;

        public ClientLocationService(HttpClient http)
        {
            _http = http;
        }
        public async Task<bool> BindSeatLayoutAsync(Guid locationId, Guid seatLayoutId)
        {
            var response = await _http.PostAsJsonAsync("api/LayoutLocation/bind",
               new BindSeatLayoutDto { LocationId = locationId, SeatLayoutId = seatLayoutId });

            return response.IsSuccessStatusCode;
        }

        public async Task<LocationDto?> CreateAsync(CreateLocationDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/LayoutLocation/create", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<LocationDto>();
        }

        public async Task<List<LocationDto>> GetAllLocationsAsync()
        {
            return await _http.GetFromJsonAsync<List<LocationDto>>("api/LayoutLocation")
                   ?? new List<LocationDto>();
        }

        public async Task<LocationDto?> GetLocationWithSeatLayoutsAsync(Guid locationId)
        {
            return await _http.GetFromJsonAsync<LocationDto>($"api/LayoutLocation/{locationId}");
        }

        public async Task<bool> UnbindSeatLayoutAsync(Guid locationId, Guid seatLayoutId)
        {
            var response = await _http.PostAsJsonAsync("api/LayoutLocation/unbind",
                new BindSeatLayoutDto { LocationId = locationId, SeatLayoutId = seatLayoutId });

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLocationStatusAsync(Guid locationId, bool isActive)
        {
            var response = await _http.PatchAsync($"api/LayoutLocation/{locationId}/status?isActive={isActive}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
