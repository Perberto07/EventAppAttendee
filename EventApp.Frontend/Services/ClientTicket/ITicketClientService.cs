using EventApp.Shared.DTOs.DtoTicket;

namespace EventApp.Frontend.Services.ClientTicket
{
    public interface ITicketClientService
    {
        Task<List<TicketDto>> GetTicketsByEventAsync(Guid eventId);
    }
}
