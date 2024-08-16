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
                    Image = e.Voucher.Image,
                    Value = e.Voucher.Value,
                    Description = e.Voucher.Description,
                    ExpireDate = e.Voucher.ExpireDate,
                    Status = e.Voucher.Status
                }).ToListAsync();
        }

        public async Task<EventWithVoucherVM> GetEventWithVoucher(int id) {
            var @event = await context.Events.Include(e => e.Vouchers).FirstOrDefaultAsync(e => e.Id == id);
            if (@event == null)
                throw new KeyNotFoundException();

            return new EventWithVoucherVM {
                Id = @event.Id,
                Name = @event.Name,
                EndDate = @event.EndDate,
                GameId = @event.GameId,
                Image = @event.Image,
                NoVoucher = @event.NoVoucher,
                StartDate = @event.StartDate,
                Vouchers = @event.Vouchers.Select(ev => new VoucherVM {
                    Id = ev.Voucher.Id,
                    Image = ev.Voucher.Image,
                    Value = ev.Voucher.Value,
                    Description = ev.Voucher.Description,
                    ExpireDate = ev.Voucher.ExpireDate,
                    Status = ev.Voucher.Status
                }).ToList(),
            };
        }

        public async Task<IEnumerable<RedeemVoucherVM>> GetBrandRedeemVoucher(int id) {
            return await context.RedeemVouchers
                .Where(e => e.Event.BrandId == id)
                .Select(e => new RedeemVoucherVM {
                    Id = e.Id,
                    Image = e.BaseVoucher.Image,
                    Description = e.BaseVoucher.Description,
                    ExpireDate = e.ExpireDate,
                    RedeemCode = e.RedeemCode,
                    RedeemTime = e.RedeemTime,
                    Value = e.BaseVoucher.Value
                }).ToListAsync();
        }

        public async Task<IEnumerable<PlayerVM>> GetBrandPlayer(int id) {
            return await context.EventPlayer
                .Where(e => e.Event.BrandId == id)
                .Select(e => new PlayerVM {
                    Email = e.Player.Email,
                    Name = e.Player.Name,
                }).ToListAsync();
        }

        public async Task<IEnumerable<PlayerVM>> GetBrandPlayerByDate(int id, DateTime start, DateTime end) {
            return await context.EventPlayer
                .Where(e => e.Event.BrandId == id 
                    && (e.Player.LastAccessed >= start && e.Player.LastAccessed < end))
                .Select(e => new PlayerVM {
                    Email = e.Player.Email,
                    Name = e.Player.Name,
                }).ToListAsync();
        }

        public async Task<IEnumerable<RedeemVoucherVM>> GetEventRedeemVoucher(int id) {
            return await context.RedeemVouchers
                .Where(e => e.EventId == id)
                .Select(e => new RedeemVoucherVM {
                    Id = e.Id,
                    Image = e.BaseVoucher.Image,
                    Description = e.BaseVoucher.Description,
                    ExpireDate = e.ExpireDate,
                    RedeemCode = e.RedeemCode,
                    RedeemTime = e.RedeemTime,
                    Value = e.BaseVoucher.Value
                }).ToListAsync();
        }

        public async Task<IEnumerable<PlayerVM>> GetEventPlayer(int id) {
            return await context.EventPlayer
                .Where(e => e.EventId == id)
                .Select(e => new PlayerVM {
                    Email = e.Player.Email,
                    Name = e.Player.Name,
                }).ToListAsync();
        }

        public async Task<IEnumerable<GameVM>> GetGame() {
            return await context.Games
                .Select(e => new GameVM {
                    Id = e.Id,
                    Name = e.Name,
                    Image = e.Image,
                })
                .ToListAsync();
        }
    }
}
