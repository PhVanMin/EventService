using EventService.API.Application.Commands;
using EventService.API.Application.Commands.EventCommands;
using EventService.API.Application.Commands.VoucherCommands;
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

        [HttpPut("{id}/Vouchers")]
        public async Task<IActionResult> UpdateEventVouchers(int id, UpdateEventVouchersRequest request) {
            var command = new UpdateEventVouchersCommand(request.voucherIds, id);
            var updateEventVouchersOrder = new IdentifiedCommand<UpdateEventVouchersCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                updateEventVouchersOrder.GetType().Name,
                nameof(updateEventVouchersOrder.Id),
                updateEventVouchersOrder.Id,
                updateEventVouchersOrder
            );

            var result = await _services.Mediator.Send(updateEventVouchersOrder);

            if (result) {
                _services.Logger.LogInformation("UpdateEventVouchersCommand succeeded");
                return NoContent();
            } else {
                _services.Logger.LogWarning("UpdateEventVouchersCommand failed");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEventForBrand(CreateEventCommand command) {
            var createEventOrder = new IdentifiedCommand<CreateEventCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                createEventOrder.GetType().Name,
                nameof(createEventOrder.Id),
                createEventOrder.Id,
                createEventOrder
            );

            var result = await _services.Mediator.Send(createEventOrder);

            if (result) {
                _services.Logger.LogInformation("CreateEventCommand succeeded");
                return Ok();
            } else {
                _services.Logger.LogWarning("CreateEventCommand failed");
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

        public record UpdateEventVouchersRequest(
            List<int> voucherIds
        );
    }
}
