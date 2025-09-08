using Blazored.LocalStorage;
using EventApp.Shared.DTOs.Event;
using EventApp.Shared.Enums;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.Event
{
    public class EventService : IEventService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public EventService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<Guid?> CreateEventAsync(CreateEventDto dto)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.PostAsJsonAsync("api/event", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Guid>();
            }

            return null;
        }

        public async Task<List<EventDto>> GetAllEventsAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.GetAsync("api/event");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<EventDto>>() ?? new();
            }

            return new List<EventDto>();
        }

        public async Task<bool> ApproveEventAsync(Guid eventId)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.PutAsync($"api/event/{eventId}/approve", null);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RejectEventAsync(Guid eventId)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.PutAsync($"api/event/{eventId}/reject", null);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEventStatusAsync(Guid eventId, EventStatus newStatus)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var dto = new UpdateEventStatusDto { Status = newStatus };
            var response = await _http.PutAsJsonAsync($"api/event/status/{eventId}", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<AvailabilityDto?> CheckAsync(DateTime start, DateTime end, Guid locationId, Guid? excludeEventId = null)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrWhiteSpace(token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            // Include locationId & excludeEventId in query
            var url = $"api/event/check-availability?start={start:o}&end={end:o}&locationId={locationId}";
            if (excludeEventId != null)
                url += $"&excludeEventId={excludeEventId}";

            return await _http.GetFromJsonAsync<AvailabilityDto>(url);
        }

        public async Task<List<EventDto>> GetUpcomingEventsAsync()
        {
            return await _http.GetFromJsonAsync<List<EventDto>>("api/event/upcoming")
           ?? new List<EventDto>();
        }

        public async Task<bool> UpdateEventAsync(Guid eventId, UpdateEventDto dto)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.PutAsJsonAsync($"api/event/{eventId}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEventAsync(Guid eventId)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrWhiteSpace(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.DeleteAsync($"api/event/{eventId}");

            return response.IsSuccessStatusCode;
        }
    }
}
