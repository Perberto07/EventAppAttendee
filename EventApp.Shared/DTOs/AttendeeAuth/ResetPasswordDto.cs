﻿

namespace EventApp.Shared.DTOs.AttendeeAuth
{
    public class ResetPasswordDto
    {
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
