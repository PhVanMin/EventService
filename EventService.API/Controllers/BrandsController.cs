using EventService.API.Application.Commands;
using EventService.API.Application.Commands.BrandCommands;
using EventService.API.Application.Queries;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase {
        private readonly EventContext _context;
        private readonly EventAPIService _services;
        public BrandsController(EventContext context, EventAPIService services) {
            _context = context;
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandVM>> GetBrandById(int id) {
            try {
                var brand = await _services.Queries.GetBrand(id);
                return Ok(brand);
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{id}/Events")]
        public async Task<ActionResult<IEnumerable<EventVM>>> GetEventsByBrandId(int id) {
            try {
                var brand = await _services.Queries.GetBrandEvent(id);
                return Ok(brand);
            } catch {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, UpdateBrandRequest request) {
            var command = new UpdateBrandCommand(id, request.Name, request.Field, request.Status);

            var updateBrandOrder = new IdentifiedCommand<UpdateBrandCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                updateBrandOrder.GetType().Name,
                nameof(updateBrandOrder.Id),
                updateBrandOrder.Id,
                updateBrandOrder
            );

            var result = await _services.Mediator.Send(updateBrandOrder);

            if (result) {
                _services.Logger.LogInformation("UpdateBrandCommand succeeded");
                return NoContent();
            } else {
                _services.Logger.LogWarning("UpdateBrandCommand failed");
                return BadRequest();
            }
        }

        public record UpdateBrandRequest(
            string Name,
            string Field,
            short Status
        );
    }
}
