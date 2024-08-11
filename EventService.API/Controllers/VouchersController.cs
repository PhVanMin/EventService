using EventService.API.Application.Commands;
using EventService.API.Application.Commands.EventCommands;
using EventService.API.Application.Commands.VoucherCommands;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using static EventService.API.Controllers.EventsController;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase {
        private readonly EventContext _context;
        private readonly EventAPIService _services;
        public VouchersController(EventContext context, EventAPIService services) {
            _context = context;
            _services = services;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucher(CreateVoucherCommand command) {
            var createVoucherOrder = new IdentifiedCommand<CreateVoucherCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                createVoucherOrder.GetType().Name,
                nameof(createVoucherOrder.Id),
                createVoucherOrder.Id,
                createVoucherOrder
            );

            var result = await _services.Mediator.Send(createVoucherOrder);

            if (result) {
                _services.Logger.LogInformation("CreateVoucherCommand succeeded");
                return Ok();
            } else {
                _services.Logger.LogWarning("CreateVoucherCommand failed");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrandVoucher(int id, UpdateVoucherRequest request) {
            var command = new UpdateVoucherCommand(
                id, request.image, request.description, request.value,
                request.expireDate, request.status);

            var updateVoucherOrder = new IdentifiedCommand<UpdateVoucherCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                updateVoucherOrder.GetType().Name,
                nameof(updateVoucherOrder.Id),
                updateVoucherOrder.Id,
                updateVoucherOrder
            );

            var result = await _services.Mediator.Send(updateVoucherOrder);

            if (result) {
                _services.Logger.LogInformation("UpdateVoucherRequest succeeded");
                return NoContent();
            } else {
                _services.Logger.LogWarning("UpdateVoucherRequest failed");
                return BadRequest();
            }
        }

        public record UpdateVoucherRequest(
            string image,
            int value,
            string description,
            int expireDate,
            int status
        );
    }
}
