

namespace EventApp.Shared.DTOs.Auth.OTPRegister
{
    public class RegisterWithOtpRequest : RegisterRequestDto
    {
        public string Otp {  get; set; } = string.Empty;
    }
}
