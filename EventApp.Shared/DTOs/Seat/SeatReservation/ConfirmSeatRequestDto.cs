

namespace EventApp.Shared.DTOs.Seat.SeatReservation
{
    public class ConfirmSeatRequestDto
    {
        public Guid EventSeatId { get; set; }
        public Guid AttendeeId { get; set; }
    }
}
