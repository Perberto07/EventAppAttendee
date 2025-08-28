

namespace EventApp.Shared.DTOs.Seat.SeatReservation
{
    public class ReserveSeatRequestDto
    {
        public Guid EventSeatId { get; set; }
        public Guid AttendeeId { get; set; }
    }
}
