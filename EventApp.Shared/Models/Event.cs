using EventApp.Shared.Enums;



namespace EventApp.Shared.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; } = "Jocfer Building";
        public Guid OrganizerId { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Pending;

        public Decimal Price { get; set; }

        public Guid SeatLayoutId { get; set; }
        public SeatLayout? SeatLayout { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<EventSeat> EventSeats { get; set; } = new List<EventSeat>();
    }
}
