using EventService.Domain.AggregatesModel.BrandAggregate;
using EventService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventService.Infrastructure.Repository {
    public class BrandRepository : IBrandRepository {
        public IUnitOfWork UnitOfWork => _context;
        private readonly EventContext _context;

        public BrandRepository(EventContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Brand AddBrand(Brand brand) {
            if (brand.IsTransient()) {
                return _context.Brands.Add(brand).Entity;
            }

            return brand;
        }

        public void DeleteBrand(Brand brand) {
            _context.Brands.Remove(brand);
        }

        public async Task<IEnumerable<Brand>> Get() {
            var brands = await _context.Brands.ToListAsync();
            return brands;
        }

        public async Task<Brand> GetByIdAsync(long id) {
            var brand = await _context.Brands.FindAsync(id);

            if (brand != null) {
                await _context.Entry(brand)
                    .Collection(i => i.Events).LoadAsync();
            }

            return brand;
        }

        public void UpdateBrand(Brand brand) {
            _context.Entry(brand).State = EntityState.Modified;
        }
    }
}
