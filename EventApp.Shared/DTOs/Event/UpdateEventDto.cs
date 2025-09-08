using EventApp.Shared.Enums;

namespace EventApp.Shared.DTOs.Event
{
    public class UpdateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal Price { get; set; }
        public EventStatus Status { get; set; }
        public Guid LocationId { get; set; }
        
        public Guid SeatLayoutId { get; set; }
    }
}
