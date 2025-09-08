using EventApp.Data;
using EventApp.Services.LocationServ;
using EventApp.Shared.DTOs.Layout;
using EventApp.Shared.DTOs.Location;
using EventApp.Shared.DTOs.Seat;
using EventApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.NewLocationService
{
    public class LocationServ : ILocationServ
    {
        private readonly DataContext _context;

        public LocationServ(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> BindSeatLayoutAsync(Guid locationId, Guid seatLayoutId)
        {
            var location = await _context.Locations
                .Include(l => l.SeatLayouts)
                .FirstOrDefaultAsync(l => l.Id == locationId && l.IsActive && !l.IsDeleted);

            if (location == null) return false;

            var seatLayout = await _context.SeatLayouts
                .FirstOrDefaultAsync(sl => sl.Id == seatLayoutId && sl.IsActive && !sl.IsDeleted);

            if (seatLayout == null) return false;

            if(!location.SeatLayouts.Any(sl => sl.Id == seatLayoutId))
            {
                location.SeatLayouts.Add(seatLayout);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
        {
            var entity = new Location
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Address = dto.Address,
            };
            _context.Locations.Add(entity);
            await _context.SaveChangesAsync();

            return new LocationDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                SeatLayouts = new List<SeatLayoutDto>()
            };
        }

        public async Task<LocationDto?> GetLocationWithSeatLayoutsAsync(Guid locationId)
        {
            var location = await _context.Locations
                 .Include(l => l.SeatLayouts)
                 .FirstOrDefaultAsync(l => l.Id == locationId && l.IsActive && !l.IsDeleted);

            if (location == null) return null;

            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                SeatLayouts = location.SeatLayouts.Select(sl => new SeatLayoutDto
                {
                    Id = sl.Id,
                    Name = sl.Name,
                    Price = sl.Price,
                    IsActive = sl.IsActive,
                    Sections = sl.LayoutSections.Select(sec => new LayoutSectionDto
                    {
                        Id = sec.Id,
                        SeatLayoutId = sec.SeatLayoutId,
                        Name = sec.Name,
                        Rows = sec.Rows,
                        Columns = sec.Columns,
                        TotalSeats = sec.TotalSeats,
                        PositionX = sec.positionX,
                        IsActive = sec.isActive
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<bool> UnbindSeatLayoutAsync(Guid locationId, Guid seatLayoutId)
        {
            var location = await _context.Locations
                 .Include(l => l.SeatLayouts)
                 .FirstOrDefaultAsync(l => l.Id == locationId);

            if (location == null) return false;

            var seatLayout = location.SeatLayouts.FirstOrDefault(sl => sl.Id == seatLayoutId);
            if (seatLayout == null) return false;

            location.SeatLayouts.Remove(seatLayout);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<List<LocationDto>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .Where(l => !l.IsDeleted && l.IsActive)
                .Select(l => new LocationDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Address = l.Address,
                    IsActive = l.IsActive,
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateLocationStatusAsync(Guid locationId, bool isActive)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);

            if (location == null) return false;

            location.IsActive = isActive;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
