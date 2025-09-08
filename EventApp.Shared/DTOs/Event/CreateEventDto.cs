using EventApp.Shared.Enums;
using EventApp.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace EventApp.Shared.DTOs.Event
{
    public class CreateEventDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime StartDateTime { get; set; } = DateTime.UtcNow;
        public DateTime EndDateTime { get; set; } = DateTime.UtcNow;
        public EventStatus Status { get; set; } = EventStatus.Pending;
        [Required]
        public Decimal Price { get; set; }

        public Guid SeatLayoutId { get; set; }
        public Guid? LocationId { get; set; }
    }
}
