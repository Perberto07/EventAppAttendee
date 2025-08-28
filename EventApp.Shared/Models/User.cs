using EventApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EventApp.Shared.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(20)")]
        public UserRole Role { get; set; } = UserRole.Organizer;

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }
    }
}
