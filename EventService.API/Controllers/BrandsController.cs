using EventService.API.DTOs.Brand;
using EventService.Domain.Model;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Controllers {
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
            var brand = new Brand() {
                Address = request.Address,
                Name = request.Name,
                Status = request.Status,
                Field = request.Field,
                Gps = request.Gps
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

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
