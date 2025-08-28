
namespace EventApp.Shared.DTOs.Auth.OTPRegister
{
    public class CompleteRegistrationDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
