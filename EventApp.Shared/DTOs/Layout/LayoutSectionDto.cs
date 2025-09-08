

namespace EventApp.Shared.DTOs.Layout
{
    public class LayoutSectionDto
    {
        public int Id { get; set; }
        public Guid SeatLayoutId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TotalSeats { get; set; }
        public int PositionX { get; set; }
        public bool IsActive { get; set; }
    }
}
