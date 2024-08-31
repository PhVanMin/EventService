using EventService.API.Application.Commands;
using EventService.API.Application.Commands.BrandCommands;
using EventService.API.Application.Queries;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase {
        private readonly EventDbContext _context;
        private readonly EventAPIService _services;
        public BrandsController(EventDbContext context, EventAPIService services) {
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
                var events = await _services.Queries.GetBrandEvent(id);
                return Ok(events);
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{id}/Vouchers")]
        public async Task<ActionResult<IEnumerable<VoucherVM>>> GetVouchersByBrandId(int id) {
            try {
                var vouchers = await _services.Queries.GetBrandVoucher(id);
                return Ok(vouchers);
            } catch {
                return NotFound();
            }
        }

        [HttpGet("{id}/Redeem")]
        public async Task<ActionResult<IEnumerable<VoucherVM>>> GetRedeemVouchersByBrandId(int id) {
            try {
                var vouchers = await _services.Queries.GetBrandVoucher(id);
                return Ok(vouchers);
            } catch {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, UpdateBrandRequest request) {
            var command = new UpdateBrandCommand(id, request.Name, request.Field, request.Status);

            var updateBrandOrder = new IdentifiedCommand<UpdateBrandCommand, bool>(command, Guid.NewGuid());

            var result = await _services.Mediator.Send(updateBrandOrder);

            if (result) {
                _services.Logger.LogInformation("UpdateBrandCommand succeeded");
                return NoContent();
            } else {
                _services.Logger.LogWarning("UpdateBrandCommand failed");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandCommand command) {
            var createBrandOrder = new IdentifiedCommand<CreateBrandCommand, bool>(command, Guid.NewGuid());
            var result = await _services.Mediator.Send(createBrandOrder);

            if (result) {
                _services.Logger.LogInformation("CreateBrandCommand succeeded");
                return NoContent();
            } else {
                _services.Logger.LogWarning("CreateBrandCommand failed");
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
