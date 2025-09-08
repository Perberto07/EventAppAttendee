

namespace EventApp.Shared.DTOs.NewEvent
{
    public class EventLayoutDto
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }
        public List<EventLayoutSectionDto> Sections { get; set; } = new();
    }
}
