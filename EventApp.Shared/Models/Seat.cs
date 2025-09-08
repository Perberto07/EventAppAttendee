

namespace EventApp.Shared.Models
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int RowNumber { get; set; }   // Numeric row (1, 2, 3...)
        public int ColumnNumber { get; set; }
        public string SeatNumber { get; set; } = default!;

        public Guid SeatLayoutId { get; set; }
        public SeatLayout SeatLayout { get; set; }

    }
}
