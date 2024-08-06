using EventService.API.Application.Commands;
using EventService.API.Application.Commands.VoucherCommands;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
    }
}
