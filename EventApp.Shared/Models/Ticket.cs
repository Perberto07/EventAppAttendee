

namespace EventApp.Shared.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public DateTime PurchaseDate { get; set; } 

        // Relationship to Attendee
        public Guid AttendeeId { get; set; }
        public AttendeeUser Attendee { get; set; } = default!;

        // Relationship to Event
        public Guid EventId { get; set; }
        public Event Event { get; set; } = default!;

        //relationship to EventLayoutSection
        public int EventLayoutSectionId { get; set; }
        public EventLayoutSection EventLayoutSection { get; set; } = default!;

        //public Guid EventSeatId { get; set; }
        //public EventSeat EventSeat { get; set; } = default!;
    }
}
