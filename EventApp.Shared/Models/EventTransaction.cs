
namespace EventApp.Shared.Models
{
    public class EventTransaction
    {
        public Guid id {  get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Guid UserId{ get; set; }
        public User User { get; set; }
        public decimal price { get; set; }
        public bool isApprove { get; set; } = false;
        public bool isPaid { get; set; } = false ;
        public bool isActive { get; set; } = true ;
    }
}
