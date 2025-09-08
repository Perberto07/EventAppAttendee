

namespace EventApp.Shared.Models
{
    public class EventLayout
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; } = default!;
        public List<EventLayoutSection> EventSections { get; set; } = new List<EventLayoutSection>();
    }
}
