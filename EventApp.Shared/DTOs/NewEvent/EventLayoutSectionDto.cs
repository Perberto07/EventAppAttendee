using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.NewEvent
{
    public class EventLayoutSectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TotalSeats { get; set; }
        public int PositionX { get; set; }
        public decimal Price { get; set; }
    }
}
