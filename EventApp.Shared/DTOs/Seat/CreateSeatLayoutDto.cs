using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.Seat
{
    public class CreateSeatLayoutDto
    {
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Decimal Price { get; set; }
    }
}
