using EventApp.Data;
using EventApp.Shared.DTOs.Layout;
using EventApp.Shared.DTOs.Seat;
using EventApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace EventApp.Services.LayoutService
{
    public class LayoutServ : ILayoutServ
    {
        private readonly DataContext _context;

        public LayoutServ(DataContext context)
        {
            _context = context;
        }

        public async Task<LayoutSectionDto?> CreateLayoutSectionAsync(CreateLayoutSectionDto dto)
        {
            var seatLayout = await _context.SeatLayouts
                .FirstOrDefaultAsync(sl => sl.Id == dto.SeatLayoutId && sl.IsActive && !sl.IsDeleted);

            if (seatLayout == null) return null;

            var section = new LayoutSection
            {
                SeatLayoutId = seatLayout.Id,
                Name = dto.Name,
                Rows = dto.Rows,
                Columns = dto.Columns,
                TotalSeats = dto.Rows * dto.Columns,
                positionX = dto.PositionX,
                isActive = true
            };

            _context.LayoutSections.Add(section);
            await _context.SaveChangesAsync();

            return new LayoutSectionDto
            {
                Id = section.Id,
                SeatLayoutId = section.SeatLayoutId,
                Name = section.Name,
                Rows = section.Rows,
                Columns = section.Columns,
                TotalSeats = section.TotalSeats,
                PositionX = section.positionX,
                IsActive = section.isActive
            };
        }

        public  async Task<SeatLayoutDto> CreateSeatLayoutWithSectionsAsync(CreateSeatLayoutSectionDto dto)
        {
            var seatLayout = new SeatLayout
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                IsActive = true,
                IsDeleted = false
            };

            foreach (var sectionDto in dto.Sections)
            {
                var section = new LayoutSection
                {
                    Name = sectionDto.Name,
                    Rows = sectionDto.Rows,
                    Columns = sectionDto.Columns,
                    TotalSeats = sectionDto.Rows * sectionDto.Columns,
                    positionX = sectionDto.PositionX,
                    isActive = true,
                    SeatLayout = seatLayout
                };
                seatLayout.LayoutSections.Add(section);
            }
            _context.SeatLayouts.Add(seatLayout);
            await _context.SaveChangesAsync();

            return new SeatLayoutDto
            {
                Id = seatLayout.Id,
                Name = seatLayout.Name,
                Price = seatLayout.Price,
                IsActive = seatLayout.IsActive,
                Sections = seatLayout.LayoutSections.Select(s => new LayoutSectionDto
                {
                    Id = s.Id,
                    SeatLayoutId = s.SeatLayoutId,
                    Name = s.Name,
                    Rows = s.Rows,
                    Columns = s.Columns,
                    TotalSeats = s.TotalSeats,
                    PositionX = s.positionX,
                    IsActive = s.isActive
                }).ToList()
            };
        }

        public async Task<List<LayoutSectionDto>> GetActiveSectionsByLayoutAsync(Guid seatLayoutId)
        {
            return await _context.LayoutSections
                .Where(ls => ls.SeatLayoutId == seatLayoutId && ls.isActive)
                .Select(ls => new LayoutSectionDto
                {
                    Id = ls.Id,
                    Name = ls.Name,
                    SeatLayoutId = ls.SeatLayoutId,
                    Rows = ls.Rows,
                    Columns = ls.Columns,
                    PositionX = ls.positionX,
                    TotalSeats = ls.TotalSeats,
                    IsActive = ls.isActive
                })
                .ToListAsync();
        }

        public async Task<List<SeatLayoutDto>> GetAllSeatLayoutsAsync()
        {
            return await _context.SeatLayouts
                .Where(sl => sl.IsActive && !sl.IsDeleted)
                .Select(sl => new SeatLayoutDto
                {
                    Id = sl.Id,
                    Name = sl.Name,
                    IsActive = sl.IsActive,
                    Price = sl.Price
                })
                .ToListAsync();
        }

        public async Task<SeatLayoutDto?> GetSeatLayoutWithSectionsAsync(Guid seatLayoutId)
        {
            var seatLayout = await _context.SeatLayouts
                .Include(sl => sl.LayoutSections)
                .FirstOrDefaultAsync(sl => sl.Id == seatLayoutId && !sl.IsDeleted);

            if (seatLayout == null) return null;

            return new SeatLayoutDto
            {
                Id = seatLayout.Id,
                Name = seatLayout.Name,
                Price = seatLayout.Price,
                IsActive = seatLayout.IsActive,
                Sections = seatLayout.LayoutSections.Select(sec => new LayoutSectionDto
                {
                    Id = sec.Id,
                    SeatLayoutId = sec.SeatLayoutId,
                    Name = sec.Name,
                    Rows = sec.Rows,
                    Columns = sec.Columns,
                    PositionX = sec.positionX,
                    TotalSeats = sec.TotalSeats,
                    IsActive = sec.isActive
                }).ToList()
            };
        }
    }
}
