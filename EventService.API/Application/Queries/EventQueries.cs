using EventService.Domain.AggregateModels.BrandAggregate;
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

        public async Task<IEnumerable<VoucherVM>> GetEventVoucher(int id) {
            return await context.EventVoucher
                .Where(ev => ev.EventId == id)
                .Select(e => new VoucherVM {
                Id = e.Voucher.Id,
                Code = e.Voucher.Code,
                Image = e.Voucher.Image,
                Value = e.Voucher.Value,
                Description = e.Voucher.Description,
                ExpireDate = e.Voucher.ExpireDate,
                Status = e.Voucher.Status
            }).ToListAsync();
        }

        public async Task<EventWithVoucherVM> GetEventWithVoucher(int id) {
            var @event = await context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (@event == null)
                throw new KeyNotFoundException();

            var eventVouchers = await GetEventVoucher(id);
            return new EventWithVoucherVM {
                Id = @event.Id,
                Name = @event.Name,
                EndDate = @event.EndDate,
                GameId = @event.GameId,
                Image = @event.Image,
                NoVoucher = @event.NoVoucher,
                StartDate = @event.StartDate,
                Vouchers = eventVouchers as List<VoucherVM> ?? new List<VoucherVM>()
            };
        }
    }
}
