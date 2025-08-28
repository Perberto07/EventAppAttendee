using EventApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.Seat
{
    public class SeatUpdateDto
    {
        public Guid EventId { get; set; }
        public Guid EventSeatId { get; set; }
        public SeatReservationStatus SeatStatus { get; set; }
        public bool IsBooked { get; set; }
    }
}
