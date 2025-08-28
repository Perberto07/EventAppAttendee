namespace EventApp.Shared.DTOs.DtoTicket
{
    public class CreateTicketDto
    {
        public Guid AttendeeId { get; set; }
        public Guid EventId { get; set; }
        public Guid EventSeatId { get; set; }
    }
}
