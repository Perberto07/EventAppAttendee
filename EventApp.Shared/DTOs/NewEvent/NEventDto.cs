using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.NewEvent
{
    public class NEventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal Price { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;

        public EventLayoutDto? EventLayout { get; set; }
    }
}
