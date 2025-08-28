using EventApp.Shared.DTOs.Seat;


namespace EventApp.Services.SeatService
{
    public interface ISeatService
    {
        Task<SeatSummaryDto> GetSeatsByEventAsync(Guid eventId);
    }
}
