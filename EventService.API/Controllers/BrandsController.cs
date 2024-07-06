using EventService.Domain.AggregatesModel.BrandAggregate;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase {
        private readonly EventContext _context;

        public BrandsController(EventContext context) {
            _context = context;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands() {
            return await _context.Brands.ToListAsync();
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
        public async Task<ActionResult<Brand>> PostBrand(BrandDTO brandDTO) {
            var brand = new Brand() {
                Name = brandDTO.Name,
                Field = brandDTO.Field,
                Address = brandDTO.Address,
                Status = brandDTO.Status,
                Gps = brandDTO.Gps
            };

            _context.Brands.Add(brand);
            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                if (BrandExists(brand.Id)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return CreatedAtAction("GetBrand", new { id = brand.Id }, brand);
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
