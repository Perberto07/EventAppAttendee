namespace EventApp.Shared.DTOs.DtoTicket
{
    public class CreateStripeSessionRequest
    {
        public Guid EventId { get; set; }
        public Guid SeatId { get; set; }
        public Guid AttendeeId { get; set; }
    }
}
