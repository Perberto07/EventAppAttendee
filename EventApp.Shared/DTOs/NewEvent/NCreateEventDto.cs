

using EventApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventApp.Shared.DTOs.NewEvent
{
    public class NCreateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; } = DateTime.UtcNow;
        public DateTime EndDateTime { get; set; } = DateTime.UtcNow;
        public Guid OrganizerId { get; set; }
        public decimal Price { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Pending;

        public Guid LocationId { get; set; }
        public Guid SeatLayoutId { get; set; }
    }
}