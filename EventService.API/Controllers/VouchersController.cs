using EventService.API.Application.Commands;
using EventService.API.Application.Commands.EventCommands;
using EventService.API.Application.Commands.VoucherCommands;
using EventService.API.Application.Queries;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase {
        private readonly EventDbContext _context;
        private readonly EventAPIService _services;
        public VouchersController(EventDbContext context, EventAPIService services) {
            _context = context;
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherVM>> GetVoucherInfo(int id) {
            try {
                var voucher = await _services.Queries.GetVoucher(id);
                return Ok(voucher);
            } catch {
                return NotFound();
            }
        }

        [HttpPost("Redeem")]
        public async Task<IActionResult> RedeemVoucher(AddRedeemVoucherOfEventCommand command) {
            try {
                var createVoucherOrder = new IdentifiedCommand<AddRedeemVoucherOfEventCommand, RedeemVoucherDataVM?>(command, Guid.NewGuid());

                var result = await _services.Mediator.Send(createVoucherOrder);

                if (result != null) {
                    _services.Logger.LogInformation("CreateVoucherCommand succeeded");
                    return Ok(result);
                } else {
                    _services.Logger.LogWarning("CreateVoucherCommand failed");
                    return BadRequest();
                }
            } catch {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromForm] CreateVoucherCommand command) {
            var createVoucherOrder = new IdentifiedCommand<CreateVoucherCommand, bool>(command, Guid.NewGuid());

            var result = await _services.Mediator.Send(createVoucherOrder);

            if (result) {
                _services.Logger.LogInformation("CreateVoucherCommand succeeded");
                return Ok();
            } else {
                _services.Logger.LogWarning("CreateVoucherCommand failed");
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBrandVoucher(int id, [FromForm] UpdateVoucherRequest request) {
            var command = new UpdateVoucherCommand(
                id, request.image, request.code, request.description, request.value,
                request.expireDate, request.status);

            var updateVoucherOrder = new IdentifiedCommand<UpdateVoucherCommand, bool>(command, Guid.NewGuid());

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
            IFormFile? image,
            string? code,
            int? value,
            string? description,
            int? expireDate,
            int? status
        );
    }
}
