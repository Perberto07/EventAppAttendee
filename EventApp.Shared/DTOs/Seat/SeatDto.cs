namespace EventApp.Shared.DTOs.Seat
{
    public class SeatDto
    {
        public Guid Id { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public bool IsBooked { get; set; }
        public Guid EventId { get; set; }
    }
}
