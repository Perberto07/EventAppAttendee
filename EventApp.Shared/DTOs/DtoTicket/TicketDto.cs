namespace EventApp.Shared.DTOs.DtoTicket
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid AttendeeId { get; set; }
        public string attendeeName { get; set; } = string.Empty;
        public string AttendeeEmail { get; set; }
        public Guid EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string Location{ get; set; } =string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
