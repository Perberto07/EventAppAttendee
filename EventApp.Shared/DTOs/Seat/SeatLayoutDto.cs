

namespace EventApp.Shared.DTOs.Seat
{
    public class SeatLayoutDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<Layout.LayoutSectionDto> Sections { get; set; } = new List<Layout.LayoutSectionDto>();
    }
}
