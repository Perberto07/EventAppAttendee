

namespace EventApp.Shared.DTOs.Layout
{
    public class SeatLayoutSectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public List<LayoutSectionDto> Sections { get; set; } = new();
    }
}
