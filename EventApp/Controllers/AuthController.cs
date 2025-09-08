using EventApp.Services.HQAuth;
using EventApp.Shared.DTOs.AttendeeAuth;
using EventApp.Shared.DTOs.Auth;
using EventApp.Shared.DTOs.Auth.OTPRegister;
using Microsoft.AspNetCore.Mvc;


namespace EventApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IOrganizerAuthService _authService;
        public AuthController(IOrganizerAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("start-registration")]
        public async Task<IActionResult> StartRegistration([FromBody] OrganizerOtpRequestDto request)
        {
            try
            {
                await _authService.StartRegistrationAsync(request.Email);
                return Ok("OTP sent to email.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("complete-registration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] RegisterWithOtpRequest dto)
        {
            try
            {
                var token = await _authService.CompleteRegistrationAsync(dto, dto.Otp);
                return Ok(token);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return Unauthorized(result.ErrorMessage);

            return Ok(result);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var success = await _authService.StartForgotPasswordAsync(dto.Email);
            if (!success)
                return NotFound("User with this email does not exist.");

            return Ok("Password reset link sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);
            if (!success)
                return BadRequest("Invalid or expired reset token.");

            return Ok("Password reset successful.");
        }
    }
}
