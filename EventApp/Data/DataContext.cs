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
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SeatLayout> SeatLayouts { get; set; }
        public DbSet<EventSeat> EventSeats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EventSeat → Seat: restrict
            modelBuilder.Entity<EventSeat>()
                .HasOne(es => es.Seat)
                .WithMany()
                .HasForeignKey(es => es.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // EventSeat → Event: cascade
            modelBuilder.Entity<EventSeat>()
                .HasOne(es => es.Event)
                .WithMany(e => e.EventSeats)
                .HasForeignKey(es => es.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ticket → Seat: restrict (prevent multiple cascade paths)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.EventSeat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.EventSeatId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ticket → Event: cascade (deleting Event deletes tickets)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ticket → Attendee: cascade if you want tickets deleted when attendee is deleted
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Attendee)
                .WithMany(a => a.Tickets)
                .HasForeignKey(t => t.AttendeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
