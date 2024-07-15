using EventService.API.Application.Commands;
using EventService.API.Application.Commands.BrandCommands;
using EventService.API.Application.Queries.BrandQueries;
using EventService.Domain.Model;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase {
        private readonly EventContext _context;
        private readonly BrandServices _services;
        public BrandsController(EventContext context, BrandServices services) {
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

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, Brand brand) {
            if (id != brand.Id) {
                return BadRequest();
            }

            _context.Entry(brand).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!BrandExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(CreateBrandCommand command) {
            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {NameProperty}: {CommandName}",
                command.GetType().Name,
                nameof(command.Name),
                command.Name
            );

            using (_services.Logger.BeginScope(new List<KeyValuePair<string, object>> { new("CreateBrandCommand", "command") })) {
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
                    _services.Logger.LogInformation("CreateOrderCommand succeeded");
                    return Ok();
                } else {
                    _services.Logger.LogWarning("CreateOrderCommand failed");
                    return BadRequest();
                }
            }
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id) {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandExists(int id) {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
