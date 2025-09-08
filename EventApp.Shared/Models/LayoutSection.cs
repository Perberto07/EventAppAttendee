namespace EventApp.Shared.Models
{
    public class LayoutSection
    {
        public int Id { get; set; }
        public Guid SeatLayoutId { get; set; }
        public SeatLayout SeatLayout { get; set; } = default!;
        public int positionX { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TotalSeats { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool isActive { get; set; } = true;
    }
}
