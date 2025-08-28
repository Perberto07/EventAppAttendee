using EventApp.Frontend.Services.SeatService;
using EventApp.Shared.DTOs.Seat;
using System.Net.Http.Json;

namespace EventApp.Client.Services.SeatService
{
    public class SeatClientService : ISeatClientService
    {
        private readonly HttpClient _http;

        public SeatClientService(HttpClient http)
        {
            _http = http;
        }

        // Fetch seats for a specific event
        public async Task<SeatSummaryDto> GetSeatsByEventAsync(Guid eventId)
        {
            var result = await _http.GetFromJsonAsync<SeatSummaryDto>($"api/seats/event/{eventId}");
            return result ?? new SeatSummaryDto
            {
                TotalSeats = 0,
                AvailableSeats = 0,
                BookedSeats = 0,
                Seats = new List<EventSeatDto>()
            };
        }

    }
}
