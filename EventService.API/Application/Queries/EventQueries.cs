using EventService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Queries {
    public class EventQueries(EventContext context) : IEventQueries {
        public async Task<IEnumerable<EventVM>> GetBrandEvent(int id) {
            return await context.Events
                .Where(e => e.BrandId == id)
                .Select(e => new EventVM {
                    Id = e.Id,
                    Name = e.Name,
                    EndDate = e.EndDate,
                    GameId = e.GameId,
                    Image = e.Image,
                    NoVoucher = e.NoVoucher,
                    StartDate = e.StartDate,
                })
                .ToListAsync();
        }

        public async Task<BrandVM> GetBrand(int id) {
            var brand = await context.Brands
                .Include(e => e.Events)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                throw new KeyNotFoundException();

            return new BrandVM {
                Id = brand.Id,
                Name = brand.Name,
                Address = brand.Location.Address,
                Gps = brand.Location.Gps,
                Field = brand.Field,
                Status = brand.Status
            };
        }

        public async Task<IEnumerable<VoucherVM>> GetBrandVoucher(int id) {
            return await context.Vouchers
                .Where(v => v.BrandId == id)
                .Select(e => new VoucherVM {
                    Id = e.Id,
                    Code = e.Code,
                    Image = e.Image,
                    Value = e.Value,
                    Description = e.Description,
                    ExpireDate = e.ExpireDate,
                    Status = e.Status
                })
                .ToListAsync();
        }
    }
}
