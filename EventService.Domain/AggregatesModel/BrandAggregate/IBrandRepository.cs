using EventService.Domain.Interfaces;

namespace EventService.Domain.AggregatesModel.BrandAggregate {
    public interface IBrandRepository : IRepository<Brand> {
        Brand AddBrand(Brand brand);
        void UpdateBrand(Brand brand);
        void DeleteBrand(Brand brand);
        Task<Brand> GetByIdAsync(long id);
        Task<IEnumerable<Brand>> Get();
    }
}
