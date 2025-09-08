using EventApp.Shared.DTOs.Seat;



namespace EventApp.Shared.DTOs.Location
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<SeatLayoutDto> SeatLayouts { get; set; } = new();
    }
}
