using System;
using System.Collections.Generic;
using System.Linq;


namespace EventApp.Shared.DTOs.Seat.SeatReservation
{
    public class SeatReservationResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
