using EventService.API.Application.Commands;
using EventService.API.Application.Commands.EventCommands;
using EventService.API.Application.Commands.VoucherCommands;
using EventService.API.Application.Queries;
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
        public async Task<IActionResult> UpdateBrandEvent(int id, UpdateEventRequest request) {
            var command = new UpdateEventCommand(
                request.brandId, id,
                request.name, request.image,
                request.noVoucher, request.start,
                request.end, request.gameId, request.voucherIds);

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

        [HttpGet("{id}/Vouchers")]
        public async Task<IActionResult> GetEventVouchers(int id) {
            try {
                var vouchers = await _services.Queries.GetEventVoucher(id);
                return Ok(vouchers);
            } catch {
                return NotFound();
            }
        }

        [HttpPost("{id}/Redeem")]
        public async Task<IActionResult> CreateRedeemVoucher(int id, CreateRedeemVoucherCommand command) {
            var createRedeemVoucherOrder = new IdentifiedCommand<CreateRedeemVoucherCommand, RedeemVoucherVM?>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                createRedeemVoucherOrder.GetType().Name,
                nameof(createRedeemVoucherOrder.Id),
                createRedeemVoucherOrder.Id,
                createRedeemVoucherOrder
            );

            var result = await _services.Mediator.Send(createRedeemVoucherOrder);
            if (result != null) {
                _services.Logger.LogInformation("CreateRedeemVoucherCommand succeeded");
                return Ok(result);
            } else {
                _services.Logger.LogWarning("CreateRedeemVoucherCommand failed");
                return BadRequest();
            }
        }

        [HttpPut("{id}/Redeem")]
        public async Task<IActionResult> RedeemVoucher(int id, RedeemVoucherCommand command) {
            var redeemVoucherOrder = new IdentifiedCommand<RedeemVoucherCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                redeemVoucherOrder.GetType().Name,
                nameof(redeemVoucherOrder.Id),
                redeemVoucherOrder.Id,
                redeemVoucherOrder
            );

            var result = await _services.Mediator.Send(redeemVoucherOrder);
            if (result) {
                _services.Logger.LogInformation("RedeemVoucherCommand succeeded");
                return Ok();
            } else {
                _services.Logger.LogWarning("RedeemVoucherCommand failed");
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
            int gameId,
            List<int> voucherIds
        );
    }
}
