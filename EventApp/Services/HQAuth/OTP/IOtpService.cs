namespace EventApp.Services.HQAuth.OTP
{
    public interface IOtpService
    {
        string GenerateAndStoreOtp(string email, TimeSpan ttl);
        bool ValidateOtp(string email, string otp);
        string IssueRegistrationToken(string email, TimeSpan ttl);
        bool TryUseRegistrationToken(string token, out string email);
    }
}
