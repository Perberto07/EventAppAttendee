using EventApp.Shared.DTOs.Auth;

namespace EventApp.Services.HQAuth
{
    public interface IOrganizerAuthService
    {
        Task<bool> StartRegistrationAsync(string email);
        Task<string> CompleteRegistrationAsync(RegisterRequestDto dto, string otp);
        Task<string> LoginAsync(LoginRequestDto dto);
        Task<bool> StartForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
