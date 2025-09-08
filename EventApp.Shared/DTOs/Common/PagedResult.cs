

using EventApp.Shared.DTOs.Seat;

namespace EventApp.Shared.DTOs.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public SeatSummaryDto? Summary { get; set; }
    }
}
