using EventApp.Shared.Enums;
using EventApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AttendeeUser> AttendeeUser { get; set; } = default!;
        public DbSet<Ticket> Tickets { get; set; } = default!;
        public DbSet<Seat> Seats { get; set; } = default!;
        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<SeatLayout> SeatLayouts { get; set; } = default!;
        public DbSet<EventSeat> EventSeats { get; set; } = default!;
        public DbSet<Location> Locations { get; set; } = default!;
        public DbSet<EventTransaction> Transactions { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<LayoutSection> LayoutSections { get; set; }
        public DbSet<EventLayout> EventLayouts { get; set; }
        public DbSet<EventLayoutSection> EventLayoutSections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EventSeat → Event

            // EventSeat → Seat  (⚠ Restrict here to prevent multiple cascade paths)
            modelBuilder.Entity<EventSeat>()
                .HasOne(es => es.Seat)
                .WithMany()   // Seat does not need collection of EventSeats
                .HasForeignKey(es => es.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket → Event (Cascade ok)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket → Attendee (Cascade ok)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Attendee)
                .WithMany(a => a.Tickets)
                .HasForeignKey(t => t.AttendeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventLayout)
                .WithOne(l => l.Event)
                .HasForeignKey<EventLayout>(l => l.EventId);
        }
    }
}
