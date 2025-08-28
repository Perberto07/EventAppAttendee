using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace EventApp.Services.HQAuth.OTP
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;
        private const string OtpKey = "otp:";
        private const string RegKey = "reg:";

        public OtpService(IMemoryCache cache) => _cache = cache;

        public string GenerateAndStoreOtp(string email, TimeSpan ttl)
        {
            int code = RandomNumberGenerator.GetInt32(100000, 1000000);
            string otp = code.ToString();
            var hash = Hash(otp);

            _cache.Set(OtpKey + email.ToLowerInvariant(), hash, ttl);
            return otp;
        }

        public bool ValidateOtp(string email, string otp)
        {
            if (!_cache.TryGetValue<byte[]>(OtpKey + email.ToLowerInvariant(), out var storedHash))
                return false;

            var providedHash = Hash(otp);
            bool ok = CryptographicOperations.FixedTimeEquals(storedHash, providedHash);

            if (ok)
                _cache.Remove(OtpKey + email.ToLowerInvariant()); // one-time use

            return ok;
        }

        public string IssueRegistrationToken(string email, TimeSpan ttl)
        {
            var token = Guid.NewGuid().ToString("N");
            _cache.Set(RegKey + token, email, ttl);
            return token;
        }

        public bool TryUseRegistrationToken(string token, out string email)
        {
            email = string.Empty;
            if (!_cache.TryGetValue<string>(RegKey + token, out var value))
                return false;

            _cache.Remove(RegKey + token);
            email = value;
            return true;
        }

        private static byte[] Hash(string input) =>
            SHA256.HashData(Encoding.UTF8.GetBytes(input));
    }
}
