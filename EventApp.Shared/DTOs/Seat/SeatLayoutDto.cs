

namespace EventApp.Shared.DTOs.Seat
{
    public class SeatLayoutDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int SeatCount { get; set; }
    }
}
