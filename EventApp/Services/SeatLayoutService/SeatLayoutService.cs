using EventApp.Data;
using EventApp.Shared.DTOs.Seat;
using EventApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.SeatLayoutService
{
    public class SeatLayoutService : ISeatLayoutService
    {
        private readonly DataContext _context;

        public SeatLayoutService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeatLayoutDto>> GetAllAsync()
        {
            return await _context.SeatLayouts
                .Select(l => new SeatLayoutDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Rows = l.Rows,
                    Columns = l.Columns,
                    SeatCount = l.Rows * l.Columns
                })
                .ToListAsync();
        }

        public async Task<SeatLayoutDto?> GetByIdAsync(Guid id)
        {
            return await _context.SeatLayouts
                .Where(l => l.Id == id)
                .Select(l => new SeatLayoutDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Rows = l.Rows,
                    Columns = l.Columns,
                    SeatCount = l.Rows * l.Columns
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateAsync(CreateSeatLayoutDto dto)
        {
            var layout = new SeatLayout
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Rows = dto.Rows,

                Columns = dto.Columns
            };

            layout.Seats = GenerateSeats(layout.Id, dto.Rows, dto.Columns);
            _context.SeatLayouts.Add(layout);
            await _context.SaveChangesAsync();

            return layout.Id;
        }



        public async Task<bool> DeleteAsync(Guid id)
        {
            var layout = await _context.SeatLayouts.FindAsync(id);
                if (layout == null)
                    return false;

            var seats = _context.Seats.Where(s => s.SeatLayoutId == id);
    
            _context.Seats.RemoveRange(seats);

            _context.SeatLayouts.Remove(layout);

            await _context.SaveChangesAsync();
            return true;
        }

        private static List<Seat> GenerateSeats(Guid layoutId, int rows, int cols)
        {
            var list = new List<Seat>(rows * cols);
            for (int r = 1; r <= rows; r++)
            {
                var rowLabel = ToLetters(r); // A, B, C, ... AA, AB, ...
                for (int c = 1; c <= cols; c++)
                {
                    list.Add(new Seat
                    {
                        Id = Guid.NewGuid(),
                        RowNumber = r,
                        ColumnNumber = c,
                        SeatNumber = $"{rowLabel}{c}",
                        SeatLayoutId = layoutId
                    });
                }
            }
            return list;
        }

        private static string ToLetters(int n)
        {
            // 1 -> A, 26 -> Z, 27 -> AA, etc.
            var sb = new System.Text.StringBuilder();
            while (n > 0)
            {
                n--;
                sb.Insert(0, (char)('A' + (n % 26)));
                n /= 26;
            }
            return sb.ToString();
        }
    }
}
