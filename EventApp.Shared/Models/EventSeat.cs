

using EventApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventApp.Shared.Models
{
    public class EventSeat
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public DateTime? ReservedUntilTime { get; set; }
        [Column("SeatStatus")]
        public SeatReservationStatus SeatStatus { get; set; } = SeatReservationStatus.Available;

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
        public Guid SeatId { get; set; }

        public Seat Seat { get; set; }
        public bool IsBooked { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
