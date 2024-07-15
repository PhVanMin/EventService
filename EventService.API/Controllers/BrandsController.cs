using EventService.API.Application.Commands;
using EventService.API.Application.Commands.BrandCommands;
using EventService.API.Application.Queries.BrandQueries;
using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase {
        private readonly EventContext _context;
        private readonly ControllerServices _services;
        public BrandsController(EventContext context, ControllerServices services) {
            _context = context;
            _services = services;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandViewModel>>> GetBrands() {
            var brands = await _services.Queries.GetAllBrand();
            return Ok(brands);
        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandViewModel>> GetBrand(int id) {
            try {
                var brand = await _services.Queries.GetBrand(id);
                return Ok(brand);
            } catch {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, UpdateBrandRequest request) {
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

        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(CreateBrandCommand command) {
            var createBrandOrder = new IdentifiedCommand<CreateBrandCommand, bool>(command, Guid.NewGuid());

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

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id) {
            var command = new DeleteBrandCommand(id);
            var deleteBrandOrder = new IdentifiedCommand<DeleteBrandCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                deleteBrandOrder.GetType().Name,
                nameof(deleteBrandOrder.Id),
                deleteBrandOrder.Id,
                deleteBrandOrder
            );

            var result = await _services.Mediator.Send(deleteBrandOrder);

            if (result) {
                _services.Logger.LogInformation("DeleteBrandCommand succeeded");
                return NoContent();
            } else {
                _services.Logger.LogWarning("DeleteBrandCommand failed");
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
