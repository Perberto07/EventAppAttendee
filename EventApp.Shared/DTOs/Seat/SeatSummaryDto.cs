

namespace EventApp.Shared.DTOs.Seat
{
    public class SeatSummaryDto
    {
        public List<EventSeatDto> Seats { get; set; } = new ();
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int BookedSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ReserveSeat { get; set; }
    }
}
