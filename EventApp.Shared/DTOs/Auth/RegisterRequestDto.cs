using EventApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;


namespace EventApp.Shared.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Organizer;
    }
}
