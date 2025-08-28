

using System.ComponentModel.DataAnnotations;

namespace EventApp.Shared.DTOs.AttendeeAuth
{
    public class RegistrationDto
    {
        public string Email { get; set; } = string.Empty; // comes from Step 1

        [Required(ErrorMessage = "Otp is required.")]
        public string Otp { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
