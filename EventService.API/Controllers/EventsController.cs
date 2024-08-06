using EventService.API.Application.Commands;
using EventService.API.Application.Commands.EventCommands;
using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase {
        private readonly EventContext _context;
        private readonly EventAPIService _services;
        public EventsController(EventContext context, EventAPIService services) {
            _context = context;
            _services = services;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrandEvent(int id, UpdateEventRequest request) {
            var command = new UpdateEventCommand(
                request.brandId, id,
                request.name, request.image,
                request.noVoucher, request.start,
                request.end, request.gameId);

            var updateBrandEventOrder = new IdentifiedCommand<UpdateEventCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                updateBrandEventOrder.GetType().Name,
                nameof(updateBrandEventOrder.Id),
                updateBrandEventOrder.Id,
                updateBrandEventOrder
            );

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
        public async Task<ActionResult<Brand>> RegisterEventForBrand(RegisterEventRequest request) {
            var command = new CreateEventCommand(
                request.brandId, request.name,
                request.image, request.noVoucher,
                request.start, request.end,
                request.gameId, request.vouchers);
            var createBrandOrder = new IdentifiedCommand<CreateEventCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                createBrandOrder.GetType().Name,
                nameof(createBrandOrder.Id),
                createBrandOrder.Id,
                createBrandOrder
            );

            var result = await _services.Mediator.Send(createBrandOrder);

            if (result) {
                _services.Logger.LogInformation("CreateBrandCommand succeeded");
                return Ok();
            } else {
                _services.Logger.LogWarning("CreateBrandCommand failed");
                return BadRequest();
            }
        }

        public record UpdateEventRequest(
            int brandId,
            string name,
            string image,
            int noVoucher,
            DateTime start,
            DateTime end,
            int gameId
        );

        public record RegisterEventRequest(
            int brandId,
            string name,
            string image,
            int noVoucher,
            DateTime start,
            DateTime end,
            int gameId,
            List<VoucherDTO> vouchers
        );
    }
}
