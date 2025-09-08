
namespace EventApp.Shared.Models
{
    public class EventLayoutSection
    {
        public int Id { get; set; }
        public int EventLayoutId { get; set; }
        public EventLayout EventLayout { get; set; } = default!;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TotalSeats { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PositionX { get; set; }
        public decimal Price { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public bool isReserved { get; set; } = false;
    }
}
