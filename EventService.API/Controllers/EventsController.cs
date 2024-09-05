using EventService.API.Application.Commands;
using EventService.API.Application.Commands.EventCommands;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase {
        private readonly EventDbContext _context;
        private readonly EventAPIService _services;
        public EventsController(EventDbContext context, EventAPIService services) {
            _context = context;
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventWithVouchers(int id) {
            try {
                var @event = await _services.Queries.GetEventWithVoucher(id);
                return Ok(@event);
            } catch {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrandEvent(int id, [FromForm] UpdateEventRequest request) {
            var command = new UpdateEventCommand(
                request.brandId, id,
                request.name, request.noVoucher, request.start,
                request.end, request.gameId, request.image, request.voucherIds);

            var updateBrandEventOrder = new IdentifiedCommand<UpdateEventCommand, bool>(command, Guid.NewGuid());

            var result = await _services.Mediator.Send(updateBrandEventOrder);

            if (result) {
                _services.Logger.LogInformation("UpdateBrandEventCommand succeeded");
                return NoContent();
            } else {
                _services.Logger.LogWarning("UpdateBrandEventCommand failed");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEventForBrand([FromForm] CreateEventCommand command) {
            var createEventOrder = new IdentifiedCommand<CreateEventCommand, bool>(command, Guid.NewGuid());

            var result = await _services.Mediator.Send(createEventOrder);

            if (result) {
                _services.Logger.LogInformation("CreateEventCommand succeeded");
                return Ok();
            } else {
                _services.Logger.LogWarning("CreateEventCommand failed");
                return BadRequest();
            }
        }

        [HttpGet("{id}/Vouchers")]
        public async Task<IActionResult> GetEventVouchers(int id) {
            try {
                var vouchers = await _services.Queries.GetEventVoucher(id);
                return Ok(vouchers);
            } catch {
                return NotFound();
            }
        }

        public record UpdateEventRequest(
            int brandId,
            string? name,
            int? noVoucher,
            DateTime? start,
            DateTime? end,
            Guid? gameId, 
            IFormFile? image,
            List<int>? voucherIds
        );
    }
}
