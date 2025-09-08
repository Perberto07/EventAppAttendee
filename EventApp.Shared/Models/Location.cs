
namespace EventApp.Shared.Models
{
    public class Location
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // One Location can have multiple SeatLayouts
        public ICollection<SeatLayout> SeatLayouts { get; set; } = new List<SeatLayout>();
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
