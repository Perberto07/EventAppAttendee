

namespace EventApp.Shared.Models
{
    public class SeatLayout
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
