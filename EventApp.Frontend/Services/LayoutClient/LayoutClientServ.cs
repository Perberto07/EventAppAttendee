using EventApp.Shared.DTOs.Layout;
using EventApp.Shared.DTOs.Seat;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace EventApp.Frontend.Services.LayoutClient
{
    public class LayoutClientServ : ILayoutClientServ
    {
        private readonly HttpClient _http;

        public LayoutClientServ(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<LayoutSectionDto?> CreateLayoutSectionAsync(CreateLayoutSectionDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/LayoutSection/create", dto);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to create section: {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadFromJsonAsync<LayoutSectionDto>();
        }

        public async Task<SeatLayoutDto?> CreateSeatLayoutWithSectionsAsync(CreateSeatLayoutSectionDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/LayoutSection/create-with-sections", dto);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to create seat layout: {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadFromJsonAsync<SeatLayoutDto>();
        }

        public async Task<List<LayoutSectionDto>> GetActiveSectionsByLayoutAsync(Guid seatLayoutId)
        {
            var result = await _http.GetFromJsonAsync<List<LayoutSectionDto>>(
               $"api/LayoutSection/active/{seatLayoutId}");

            return result ?? new List<LayoutSectionDto>();
        }

        public async Task<SeatLayoutDto?> GetSeatLayoutWithSectionsAsync(Guid seatLayoutId)
        {
            var result = await _http.GetFromJsonAsync<SeatLayoutDto>(
                           $"api/LayoutSection/{seatLayoutId}");

            return result;
        }

        public async Task<List<SeatLayoutDto>> GetAllLayoutSection()
        {
            var result = await _http.GetFromJsonAsync<List<SeatLayoutDto>>(
               $"api/LayoutSection/all");

            return result ?? new List<SeatLayoutDto>();
        }
    }
}
