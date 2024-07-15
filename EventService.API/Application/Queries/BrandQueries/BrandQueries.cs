using EventService.Domain.Model;
using EventService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Queries.BrandQueries {
    public class BrandQueries(EventContext context) : IBrandQueries {
        public async Task<IEnumerable<BrandViewModel>> GetAllBrand() {
            return await context.Brands
                .Include(b => b.Events)
                .Select(b => new BrandViewModel {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address, 
                    Gps = b.Gps,
                    Field = b.Field,
                    Status = b.Status,
                    Events = b.Events.Select(e => new BrandEventViewModel {
                        Name = e.Name,
                        Image = e.Image,
                        NoVoucher = e.NoVoucher,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        GameId = e.GameId
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<BrandViewModel> GetBrand(int id) {
            var brand = await context.Brands
                .Include(e => e.Events)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                throw new KeyNotFoundException();

            return new BrandViewModel {
                Id = brand.Id,
                Name = brand.Name,
                Address = brand.Address,
                Gps = brand.Gps,
                Field = brand.Field,
                Status = brand.Status,
                Events = brand.Events.Select(e => new BrandEventViewModel {
                    Name = e.Name,
                    Image = e.Image,
                    NoVoucher = e.NoVoucher,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    GameId = e.GameId
                }).ToList()
            };
        }
    }
}
