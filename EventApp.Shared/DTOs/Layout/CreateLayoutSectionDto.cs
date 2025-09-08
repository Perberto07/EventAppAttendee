

namespace EventApp.Shared.DTOs.Layout
{
    public class CreateLayoutSectionDto
    {
        public Guid? SeatLayoutId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int PositionX { get; set; }
    }
}
