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
        public Guid OrganizerId { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Pending;

        public Decimal Price { get; set; }

        public Guid? LocationId { get; set; }
        public Location? Location { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public int EventLayoutId { get; set; }
        public EventLayout? EventLayout { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public ICollection<EventSeat> EventSeats { get; set; } = new List<EventSeat>();
        //public Guid SeatLayoutId { get; set; }
        //public SeatLayout? SeatLayout { get; set; }
    }
}
