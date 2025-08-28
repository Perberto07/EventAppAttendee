using EventApp.Services.SeatLayoutService;
using EventApp.Shared.DTOs.Seat;
using EventApp.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatLayoutsController : ControllerBase
    {
        private readonly ISeatLayoutService _seatLayoutService;

        public SeatLayoutsController(ISeatLayoutService seatLayoutService)
        {
            _seatLayoutService = seatLayoutService;
        }

        // GET: api/SeatLayout
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SeatLayoutDto>>> GetAll()
        {
            var layouts = await _seatLayoutService.GetAllAsync();
            return Ok(layouts);
        }

        // GET: api/SeatLayout/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Organizer")]
        public async Task<ActionResult<SeatLayoutDto>> GetById(Guid id)
        {
            var layout = await _seatLayoutService.GetByIdAsync(id);
            if (layout == null)
                return NotFound();

            return Ok(layout);
        }

        // POST: api/SeatLayout
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateSeatLayoutDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _seatLayoutService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        // DELETE: api/SeatLayout/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _seatLayoutService.DeleteAsync(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
