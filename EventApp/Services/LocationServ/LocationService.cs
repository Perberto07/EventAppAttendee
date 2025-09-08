//using EventApp.Data;
//using EventApp.Shared.DTOs.Location;
//using EventApp.Shared.DTOs.Seat;
//using EventApp.Shared.Models;
//using Microsoft.EntityFrameworkCore;

//namespace EventApp.Services.LocationServ
//{
//    public class LocationService : ILocationService
//    {
//        private readonly DataContext _context;
//        public LocationServ(DataContext context) => _context = context;

//        public async Task<List<LocationDto>> GetAllAsync()
//        {
//            return await _context.Locations
//                .Select(l => new LocationDto
//                {
//                    Id = l.Id,
//                    Name = l.Name,
//                    Address = l.Address
//                })
//                .ToListAsync();
//        }

//        public async Task<LocationDto?> GetByIdAsync(Guid id)
//        {
//            var location = await _context.Locations
//                .Include(l => l.SeatLayouts) // include layouts
//                .FirstOrDefaultAsync(l => l.Id == id);

//            if (location == null) return null;

//            return new LocationDto
//            {
//                Id = location.Id,
//                Name = location.Name,
//                Address = location.Address,
//                SeatLayouts = location.SeatLayouts
//                    .Select(sl => new SeatLayoutDto
//                    {
//                        Id = sl.Id,
//                        Name = sl.Name,
//                        Rows = sl.Rows,
//                        Columns = sl.Columns
//                    }).ToList()
//            };
//        }

//        public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
//        {
//            var entity = new Location
//            {
//                Id = Guid.NewGuid(),
//                Name = dto.Name,
//                Address = dto.Address,
//            };
//            _context.Locations.Add(entity);
//            await _context.SaveChangesAsync();

//            return new LocationDto
//            {
//                Id = entity.Id,
//                Name = entity.Name,
//                Address = entity.Address,
//                SeatLayouts = new List<SeatLayoutDto>()
//            };
//        }

//        public async Task<LocationDto?> UpdateAsync(UpdateLocationDto dto)
//        {
//            var entity = await _context.Locations.FindAsync(dto.Id);
//            if (entity == null) return null;

//            entity.Name = dto.Name;
//            entity.Address = dto.Address;
//            await _context.SaveChangesAsync();

//            return new LocationDto
//            {
//                Id = entity.Id,
//                Name = entity.Name,
//                Address = entity.Address
//            };
//        }

//        public async Task<bool> DeleteAsync(Guid id)
//        {
//            var entity = await _context.Locations
//                .Include(l => l.SeatLayouts)
//                .FirstOrDefaultAsync(l => l.Id == id);

//            if (entity == null) return false;

//            _context.Locations.Remove(entity);
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        // ✅ Get seat layouts bound to a location
//        public async Task<List<SeatLayoutDto>> GetSeatLayoutsByLocationAsync(Guid locationId)
//        {
//            return await _context.SeatLayouts
//                .Where(sl => sl.Locations.Any(l => l.Id == locationId))
//                .Select(sl => new SeatLayoutDto
//                {
//                    Id = sl.Id,
//                    Name = sl.Name,
//                    Rows = sl.Rows,
//                    Columns = sl.Columns,
//                    Price = sl.Price,
//                    SeatCount = sl.Seats.Count()
//                })
//                .ToListAsync();
//        }

//        public async Task<bool> BindSeatLayoutsAsync(Guid locationId, List<Guid> seatLayoutIds)
//        {
//            var location = await _context.Locations
//                .Include(l => l.SeatLayouts)
//                .FirstOrDefaultAsync(l => l.Id == locationId);

//            if (location == null) return false;

//            var seatLayouts = await _context.SeatLayouts
//                .Where(sl => seatLayoutIds.Contains(sl.Id))
//                .ToListAsync();

//            foreach (var sl in seatLayouts)
//            {
//                if (!location.SeatLayouts.Any(x => x.Id == sl.Id))
//                {
//                    location.SeatLayouts.Add(sl);
//                }
//            }

//            await _context.SaveChangesAsync();
//            return true;
//        }

//        // ✅ Unbind seat layouts from location
//        public async Task<bool> UnbindSeatLayoutsAsync(Guid locationId, List<Guid> seatLayoutIds)
//        {
//            var location = await _context.Locations
//                .Include(l => l.SeatLayouts)
//                .FirstOrDefaultAsync(l => l.Id == locationId);

//            if (location == null)
//                return false;

//            // Safely remove seat layouts that match the given IDs
//            foreach (var seatLayout in location.SeatLayouts
//                .Where(sl => seatLayoutIds.Contains(sl.Id))
//                .ToList())
//            {
//                location.SeatLayouts.Remove(seatLayout);
//            }

//            await _context.SaveChangesAsync();
//            return true;
//        }

//    }
//}
