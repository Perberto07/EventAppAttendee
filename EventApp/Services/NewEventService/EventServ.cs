using EventApp.Data;
using EventApp.Shared.DTOs.NewEvent;
using EventApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.NewEventService
{
    public class EventServ : IEventServ
    {
        private readonly DataContext _ctx;

        public EventServ(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<NEventDto?> CreateEventAsync(NCreateEventDto dto)
        {
            var location = await _ctx.Locations
                .Include(l => l.SeatLayouts)
                    .ThenInclude(sl => sl.LayoutSections)
                .FirstOrDefaultAsync(l => l.Id == dto.LocationId);

            if (location == null)
                throw new Exception("Location not found");

            var seatLayout = location.SeatLayouts
                .FirstOrDefault(sl => sl.Id == dto.SeatLayoutId);

            if (seatLayout == null)
                throw new Exception("Seat layout not found for this location");

            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                OrganizerId = dto.OrganizerId,
                LocationId = dto.LocationId
            };

            _ctx.Events.Add(ev);
            await _ctx.SaveChangesAsync();

            var eventLayout = new EventLayout
            {
                EventId = ev.Id,
            };

            _ctx.EventLayouts.Add(eventLayout);
            await _ctx.SaveChangesAsync();

            foreach (var section in seatLayout.LayoutSections)
            {
                var eventSection = new EventLayoutSection
                {
                    EventLayoutId = eventLayout.Id,
                    Rows = section.Rows,
                    Columns = section.Columns,
                    TotalSeats = section.TotalSeats,
                    Name = section.Name,
                    PositionX = section.positionX
                };

                _ctx.EventLayoutSections.Add(eventSection);
            }

            await _ctx.SaveChangesAsync();

            return new NEventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartDateTime = ev.StartDateTime,
                EndDateTime = ev.EndDateTime,
                LocationId = location.Id,
                LocationName = location.Name,
                EventLayout = new EventLayoutDto
                {
                    Id = eventLayout.Id,
                    EventId = ev.Id,
                    Sections = eventLayout.EventSections.Select(s => new EventLayoutSectionDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Rows = s.Rows,
                        Columns = s.Columns,
                        TotalSeats = s.TotalSeats,
                        PositionX = s.PositionX,
                        Price = s.Price
                    }).ToList()
                }
            };
        }
        
    }
}
