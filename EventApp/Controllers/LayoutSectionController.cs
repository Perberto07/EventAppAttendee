using EventApp.Services.LayoutService;
using EventApp.Shared.DTOs.Layout;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LayoutSectionController : ControllerBase
    {
        private readonly ILayoutServ _service;

        public LayoutSectionController(ILayoutServ service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLayoutWithSection([FromBody] CreateLayoutSectionDto dto)
        {
            var result = await _service.CreateLayoutSectionAsync(dto);
            if (result == null) return BadRequest("SeatLayout not found or inactive.");
            return Ok(result);
        }

        [HttpGet("active/{seatLayoutId}")]
        public async Task<IActionResult> GetActiveSections(Guid seatLayoutId)
        {
            var sections = await _service.GetActiveSectionsByLayoutAsync(seatLayoutId);
            return Ok(sections);
        }

        [HttpPost("create-with-sections")]
        public async Task<IActionResult> CreateWithSections([FromBody] CreateSeatLayoutSectionDto dto)
        {
            var result = await _service.CreateSeatLayoutWithSectionsAsync(dto);
            return Ok(result);
        }

        [HttpGet("{seatLayoutId}")]
        public async Task<IActionResult> GetSeatLayout(Guid seatLayoutId)
        {
            var result = await _service.GetSeatLayoutWithSectionsAsync(seatLayoutId);
            if (result == null) return NotFound("SeatLayout not found.");
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSeatLayouts()
        {
            var layouts = await _service.GetAllSeatLayoutsAsync();
            return Ok(layouts);
        }
    }
}
