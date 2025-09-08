using EventApp.Shared.DTOs.DtoTicket;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.ClientTicket
{
    public class TicketClientService : ITicketClientService
    {
        private readonly HttpClient _http;

        public TicketClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TicketDto>> GetTicketsByEventAsync(Guid eventId)
        {
            var response = await _http.GetFromJsonAsync<List<TicketDto>>($"api/tickets/ticket/{eventId}");
            return response ?? new List<TicketDto>();
        }
    }
}
