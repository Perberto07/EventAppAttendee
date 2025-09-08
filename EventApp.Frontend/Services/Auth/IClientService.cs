using EventApp.Shared.DTOs.Auth;
using EventApp.Shared.DTOs.Auth.OTPRegister;

namespace EventApp.Frontend.Services.Auth
{
    public interface IClientService
    {
        Task<LoginResultDto?> LoginAsync(LoginRequestDto dto);
        Task<string?> StartRegistrationAsync(string email);
        Task<string?> CompleteRegistrationAsync(CompleteRegistrationDto dto);
        Task LogoutAsync();
        Task<string?> GetTokenAsync();
        bool IsTokenExpired(string token);
        Task EnsureTokenValidAsync();
    }
}
