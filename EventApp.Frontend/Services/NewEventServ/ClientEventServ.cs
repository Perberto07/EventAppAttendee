using EventApp.Shared.DTOs.NewEvent;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.NewEventServ
{
    public class ClientEventServ : IClientEventServ
    {
        private readonly HttpClient _http;

        public ClientEventServ(HttpClient http)
        {
            _http = http;
        }
        public async Task<NEventDto?> CreateEventAsync(NCreateEventDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/NewEvent/create", dto);

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create event: {msg}");
            }

            return await response.Content.ReadFromJsonAsync<NEventDto>();
        }
    }
}
