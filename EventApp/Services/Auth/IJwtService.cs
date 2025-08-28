using EventApp.Shared.Models;

namespace EventApp.Services.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
