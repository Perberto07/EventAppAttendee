using EventApp.Shared.DTOs.Layout;
using EventApp.Shared.DTOs.Seat;

namespace EventApp.Frontend.Services.LayoutClient
{
    public interface ILayoutClientServ
    {
        Task<LayoutSectionDto?> CreateLayoutSectionAsync(CreateLayoutSectionDto dto);
        Task<List<LayoutSectionDto>> GetActiveSectionsByLayoutAsync(Guid seatLayoutId);
        Task<SeatLayoutDto?> CreateSeatLayoutWithSectionsAsync(CreateSeatLayoutSectionDto dto);
        Task<SeatLayoutDto?> GetSeatLayoutWithSectionsAsync(Guid seatLayoutId);
        Task<List<SeatLayoutDto>> GetAllLayoutSection();
    }
}
