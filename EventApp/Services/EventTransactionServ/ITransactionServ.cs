using EventApp.Shared.DTOs.EventTransactionDto;
using EventApp.Shared.Models;

namespace EventApp.Services.EventTransactionServ
{
    public interface ITransactionServ
    {
        Task<EventTransaction> CreateTransactionAsync(CreateEventTransactionDto dto);
        Task<List<EventTransaction>> GetUserTransactionsAsync(Guid userId);
        Task<List<EventTransaction>> GetAllTransactionsAsync();

        Task<bool> UpdateTransactionStatusByEventAsync(Guid eventId, string status);
    }
}
