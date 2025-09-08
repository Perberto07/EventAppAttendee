using EventApp.Shared.DTOs.Common;
using EventApp.Shared.DTOs.Seat;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.EventSeatService
{
    public class EventSeatService : IEventSeatService
    {
        private readonly HttpClient _http;

        public EventSeatService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PagedResult<EventSeatDto>> GetEventSeatsByEventAsync(Guid eventId, PaginationParams pagination)
        {
            var url = $"api/seats/event/{eventId}?pageNumber={pagination.PageNumber}&pageSize={pagination.PageSize}";

            var result = await _http.GetFromJsonAsync<PagedResult<EventSeatDto>>(url);

            if (result == null)
                return new PagedResult<EventSeatDto>
                {
                    Items = new List<EventSeatDto>(),
                    TotalCount = 0,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalPages = 0
                };

            return result;
        }
    }
}
