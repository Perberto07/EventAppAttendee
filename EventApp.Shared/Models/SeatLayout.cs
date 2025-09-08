

namespace EventApp.Shared.Models
{
    public class SeatLayout
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Decimal Price { get; set; }
        public ICollection<Location> Locations { get; set; } = new List<Location>();
        public ICollection<LayoutSection> LayoutSections { get; set; } = new List<LayoutSection>();
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;

        //public Guid SeatId { get; set; }
        //public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
