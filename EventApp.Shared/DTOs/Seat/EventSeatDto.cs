
using EventApp.Shared.Enums;

namespace EventApp.Shared.DTOs.Seat
{
    public class EventSeatDto
    {
        public Guid EventSeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public bool IsBooked { get; set; }
        public SeatReservationStatus SeatStatus { get; set; } 
    }
}
