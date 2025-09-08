
namespace EventApp.Shared.DTOs.Layout
{
    public class CreateSeatLayoutSectionDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Optional: define layout sections when creating
        public List<CreateSectionNoGDto> Sections { get; set; } = new();
    }
}
