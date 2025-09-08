using EventApp.Data;
using EventApp.Services.Auth;
using EventApp.Services.HQAuth.OTP;
using EventApp.Shared.DTOs.Auth;
using EventApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EventApp.Services.HQAuth
{
    public class OrganizerAuthService : IOrganizerAuthService
    {
        private readonly DataContext _context;
        private readonly IJwtService _jwtService;
        private readonly EmailService _emailService; // We'll use SMTP later
        private readonly IOtpService _otpService;

        public OrganizerAuthService(
            DataContext context,
            IJwtService jwtService,
            EmailService emailService,
            IOtpService otpService)
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
            _otpService = otpService;
        }

        public async Task<bool> StartRegistrationAsync(string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new InvalidOperationException("Email already used.");

            var otp = _otpService.GenerateAndStoreOtp(email, TimeSpan.FromMinutes(5));
            await _emailService.SendEmailAsync(email, "Your Organizer OTP", $"Your OTP is: {otp}");

            return true;
        }

        public async Task<string> CompleteRegistrationAsync(RegisterRequestDto dto, string otp)
        {
            if (!_otpService.ValidateOtp(dto.Email, otp))
                throw new InvalidOperationException("Invalid or expired OTP.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _jwtService.GenerateToken(user);
        }

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return new LoginResultDto { Success = false, ErrorMessage = "Username not found." };

            if (!VerifyPassword(dto.Password, user.PasswordHash))
                return new LoginResultDto { Success = false, ErrorMessage = "Incorrect password." };

            return new LoginResultDto
            {
                Success = true,
                Token = _jwtService.GenerateToken(user)
            };
        }


        public async Task<bool> StartForgotPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.ResetToken = Hash(rawToken);
            user.ResetTokenExpiration = DateTime.UtcNow.AddMinutes(15);

            await _context.SaveChangesAsync();

            var resetLink = $"https://localhost:7286/lander?token={WebUtility.UrlEncode(rawToken)}";

            // Send email
            await _emailService.SendEmailAsync(user.Email, "Organizer Password Reset", 
                            $"Click here to reset: <a href='{resetLink}'> Reset Password</a>");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var hashedToken = Hash(token);
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.ResetToken == hashedToken &&
                u.ResetTokenExpiration != null &&
                u.ResetTokenExpiration > DateTime.UtcNow);

            if (user == null) return false;

            user.PasswordHash = HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiration = null;

            await _context.SaveChangesAsync();
            return true;
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        private static bool VerifyPassword(string password, string hashed)
        {
            return HashPassword(password) == hashed;
        }

        private static string Hash(string value)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}
