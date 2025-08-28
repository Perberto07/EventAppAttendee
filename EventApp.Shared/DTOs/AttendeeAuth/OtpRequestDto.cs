using System.ComponentModel.DataAnnotations;

namespace EventApp.Shared.DTOs.AttendeeAuth
{
    public class OtpRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
    }
}
