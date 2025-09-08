using EventApp.Shared.DTOs.DtoTicket;

namespace EventApp.Services.TicketService
{
    public interface ITicketMonitorService
    {
        Task<List<TicketDto>> GetTicketByEventAsync(Guid eventId);
    }
}
