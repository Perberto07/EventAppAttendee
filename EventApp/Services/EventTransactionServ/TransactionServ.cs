using EventApp.Data;
using EventApp.Shared.DTOs.EventTransactionDto;
using EventApp.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace EventApp.Services.EventTransactionServ
{
    public class TransactionServ : ITransactionServ
    {
        private readonly DataContext _context;

        public TransactionServ(DataContext context)
        {
            _context = context;
        }

        public async Task<EventTransaction> CreateTransactionAsync(CreateEventTransactionDto dto)
        {
            var transaction = new EventTransaction
            {
                id = Guid.NewGuid(),
                EventId = dto.EventId,
                UserId = dto.UserId,
                price = dto.Price,       // default 0
                isApprove = false,
                isPaid = false,
                isActive = true
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }


        public async Task<List<EventTransaction>> GetUserTransactionsAsync(Guid userId)
        {
            return await _context.Transactions
                                 .Include(t => t.Event)
                                 .Include(t => t.User)
                                 .Where(t => t.UserId == userId)
                                 .ToListAsync();
        }
        public async Task<List<EventTransaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                             .Include(t => t.Event)
                             .Include(t => t.User) // optional, if you want user info
                             .OrderByDescending(t => t.Event.StartDateTime) // optional sorting
                             .ToListAsync();
        }

        public async Task<bool> UpdateTransactionStatusByEventAsync(Guid eventId, string status)
        {
            var transactions = await _context.Transactions
                .Where(t => t.EventId == eventId)
                .ToListAsync();

            if (!transactions.Any())
                return false;

            bool approve = status.Equals("Approved", StringComparison.OrdinalIgnoreCase);

            foreach (var t in transactions)
            {
                t.isApprove = approve;
            }

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
