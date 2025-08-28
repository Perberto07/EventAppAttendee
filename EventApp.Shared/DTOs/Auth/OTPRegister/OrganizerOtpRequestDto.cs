using System.ComponentModel.DataAnnotations;

namespace EventApp.Shared.DTOs.Auth.OTPRegister
{
    public class OrganizerOtpRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
