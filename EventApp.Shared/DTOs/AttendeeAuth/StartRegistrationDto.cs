

namespace EventApp.Shared.DTOs.AttendeeAuth
{
    public class StartRegistrationDto
    {
        public string Email { get; set; }
    }

    public class VerifyOtpDto 
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

    public class RegisterWithTokenDto
    {
        public string RegistrationToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
}
