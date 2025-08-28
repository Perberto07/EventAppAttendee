

namespace EventApp.Shared.DTOs.Seat
{
    public class UpdateSeatLayoutDto
    {
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
