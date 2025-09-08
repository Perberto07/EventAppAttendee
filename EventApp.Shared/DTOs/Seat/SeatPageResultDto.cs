

using EventApp.Shared.DTOs.Common;

namespace EventApp.Shared.DTOs.Seat
{
    public class SeatPageResultDto
    {
        public SeatSummaryDto Summary { get; set; } = default!;
        public PagedResult<EventSeatDto> PagedSeats { get; set; } = default!;
    }
}
