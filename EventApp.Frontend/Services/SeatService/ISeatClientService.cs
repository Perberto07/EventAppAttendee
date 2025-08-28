using EventApp.Shared.DTOs.Seat;
namespace EventApp.Frontend.Services.SeatService
{
    public interface ISeatClientService
    {
        Task<SeatSummaryDto> GetSeatsByEventAsync(Guid eventId);
    }
}
