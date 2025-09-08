using EventApp.Shared.Models;

namespace EventApp.Frontend.Services.ClientTransactionServ
{
    public interface IClientTransactionService
    {
        Task<List<EventTransaction>> GetMyTransactionsAsync();
        Task<List<EventTransaction>> GetAllTransactionsAsync();
    }
}
