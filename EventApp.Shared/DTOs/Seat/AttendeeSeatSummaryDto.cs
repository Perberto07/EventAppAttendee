

namespace EventApp.Shared.DTOs.Seat
{
    public class AttendeeSeatSummaryDto
    {
        public List<EventSeatDto> Seats { get; set; } = new ();
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int BookedSeats { get; set; }
        public int ReserveSeat { get; set; }
    }
}
