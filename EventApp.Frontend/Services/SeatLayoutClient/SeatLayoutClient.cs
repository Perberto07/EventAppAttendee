using Blazored.LocalStorage;
using EventApp.Shared.DTOs.Seat;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.SeatLayoutClient
{
    public class SeatLayoutClient : ISeatLayoutClient
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public SeatLayoutClient(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        private async Task AddAuthHeaderAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IEnumerable<SeatLayoutDto>> GetAllAsync()
        {
            await AddAuthHeaderAsync();
            var result = await _http.GetFromJsonAsync<IEnumerable<SeatLayoutDto>>("api/SeatLayouts");
            return result ?? Enumerable.Empty<SeatLayoutDto>();
        }

        public async Task<SeatLayoutDto?> GetByIdAsync(Guid id)
        {
            await AddAuthHeaderAsync();
            return await _http.GetFromJsonAsync<SeatLayoutDto>($"api/SeatLayouts/{id}");
        }

        public async Task<Guid?> CreateAsync(CreateSeatLayoutDto dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/SeatLayouts", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Guid>();
            }
            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await AddAuthHeaderAsync();
            var response = await _http.DeleteAsync($"api/SeatLayouts/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
