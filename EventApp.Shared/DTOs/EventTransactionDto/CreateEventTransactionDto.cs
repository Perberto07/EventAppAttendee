

namespace EventApp.Shared.DTOs.EventTransactionDto
{
    public class CreateEventTransactionDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; } = 0;
    }
}
