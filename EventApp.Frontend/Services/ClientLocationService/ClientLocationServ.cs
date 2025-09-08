using EventApp.Shared.DTOs.Location;
using EventApp.Shared.DTOs.Seat;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.ClientLocationService
{
    public class ClientLocationServ : IClientLocationServ
    {
        private readonly HttpClient _http;

        public ClientLocationServ(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<LocationDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<LocationDto>>("api/locations")
                   ?? new List<LocationDto>();
        }

        public async Task<LocationDto?> GetByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<LocationDto>($"api/locations/{id}");
        }

        public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/locations", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LocationDto>()
                   ?? throw new InvalidOperationException("Failed to create location");
        }

        public async Task<LocationDto?> UpdateAsync(UpdateLocationDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/locations/{dto.Id}", dto);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<LocationDto>();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/locations/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<SeatLayoutDto>> GetSeatLayoutsByLocationAsync(Guid locationId)
        {
            var layouts = await _http.GetFromJsonAsync<List<SeatLayoutDto>>(
                $"api/locations/{locationId}/seatlayouts"
            );
            return layouts ?? new List<SeatLayoutDto>();
        }

        // ✅ Bind seat layouts
        public async Task<bool> BindSeatLayoutsAsync(Guid locationId, List<Guid> seatLayoutIds)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/locations/{locationId}/seatlayouts/bind",
                seatLayoutIds
            );
            return response.IsSuccessStatusCode;
        }

        // ✅ Unbind seat layouts
        public async Task<bool> UnbindSeatLayoutsAsync(Guid locationId, List<Guid> seatLayoutIds)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/locations/{locationId}/seatlayouts/unbind",
                seatLayoutIds
            );
            return response.IsSuccessStatusCode;
        }
    }
}
