using EventService.API.Application.Commands.BrandCommands;
using EventService.API.DTOs.Brand;
using EventService.Domain.Model;
using EventService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase {
        private readonly EventContext _context;
        private readonly ILogger<BrandsController> _logger;
        private readonly IMediator _mediator;

        public BrandsController(EventContext context, ILogger<BrandsController> logger, IMediator mediator) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands() {
            return await _context.Brands.Include(b => b.Events).ToListAsync();
        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(long id) {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null) {
                return NotFound();
            }

            return brand;
        }

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(long id, Brand brand) {
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
        public async Task<ActionResult<Brand>> PostBrand(CreateBrandRequest request) {
            _logger.LogInformation(
                "Sending command: {CommandName} - {NameProperty}: {CommandName}",
                request.GetType().Name,
                nameof(request.Name),
                request.Name
            );

            using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("CreateBrandCommand", "command") })) {
                var createBrandCommand = new CreateBrandCommand(
                    request.Name, 
                    request.Field, 
                    request.Address, 
                    request.Gps, 
                    request.Status
                );

                _logger.LogInformation(
                    "Sending command: {CommandName} - {NameProperty}: {CommandName} ({@Command})",
                    createBrandCommand.GetType().Name,
                    nameof(createBrandCommand.Name),
                    createBrandCommand.Name,
                    createBrandCommand
                );

                var result = await _mediator.Send(createBrandCommand);

                if (result) {
                    _logger.LogInformation("CreateOrderCommand succeeded");
                    return Ok();
                } else {
                    _logger.LogWarning("CreateOrderCommand failed");
                    return BadRequest();
                }
            }
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(long id) {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandExists(long id) {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
