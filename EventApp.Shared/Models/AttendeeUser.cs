
namespace EventApp.Shared.Models
{
    public class AttendeeUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }

        public List<Ticket> Tickets { get; set; } = new();
    }
}
