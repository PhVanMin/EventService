using EventService.Domain.AggregateModels.PlayerAggregate;
using EventService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Queries {
    public class EventQueries(EventDbContext context) : IEventQueries {
        public async Task<IEnumerable<EventVM>> GetBrandEvent(int id) {
            var brand = await context.Brands
                .Include(b => b.Events)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (brand == null)
                throw new KeyNotFoundException();

            return brand.Events
                .Select(e => new EventVM {
                    Id = e.Id,
                    Name = e.Name,
                    EndDate = e.EndDate,
                    GameId = e.GameId,
                    Image = e.Image,
                    NoVoucher = e.NoVoucher,
                    StartDate = e.StartDate,
                })
                .ToList();
        }

        public async Task<BrandVM> GetBrand(int id) {
            var brand = await context.Brands
                .FindAsync(id);

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
            var brand = await context.Brands
                .Include(b => b.Vouchers)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                throw new KeyNotFoundException();

            return brand.Vouchers
                .Where(v => v.BrandId == id)
                .Select(e => new VoucherVM {
                    Id = e.Id,
                    Image = e.Image,
                    Value = e.Value,
                    Description = e.Description,
                    ExpireDate = e.ExpireDate,
                    Status = e.Status
                })
                .ToList();
        }

        public async Task<IEnumerable<VoucherVM>> GetEventVoucher(int id) {
            var @event = await context.Events.FindAsync(id);
            if (@event == null)
                throw new KeyNotFoundException();

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
            var @event = await context.Events
                .Include(e => e.Vouchers)
                .ThenInclude(ev => ev.Voucher)
                .FirstOrDefaultAsync(e => e.Id == id);
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

        public async Task<VoucherVM> GetVoucher(int id) {
            var voucher =  await context.Vouchers.FindAsync(id);
            if (voucher == null) {
                throw new KeyNotFoundException();
            }

            return new VoucherVM {
                Id = id,
                Image = voucher.Image,
                Value = voucher.Value,
                Description = voucher.Description,
                ExpireDate = voucher.ExpireDate,
                Status = voucher.Status,
            };
        }

        public async Task<string> GetEventVoucherCode(int eventId, int voucherId) {
            var ev = await context.EventVoucher.Include(ev => ev.Voucher).FirstOrDefaultAsync(e => e.EventId == eventId && e.VoucherId == voucherId);
            if (ev == null) {
                throw new KeyNotFoundException();
            }

            return ev.Voucher.Code;
        }

        public async Task<BrandStatisticsVM> GetBrandStatisticsByDate(int id, bool all, DateTime start, DateTime end) {
            var startDate = start.ToUniversalTime();
            var endDate = end.AddDays(1).ToUniversalTime();

            var brand = await context.Brands.FindAsync(id);
            if (brand == null)
                throw new KeyNotFoundException();

            var playersTask = context.EventPlayer
                .Include(e => e.Player)
                .Where(e => e.Event.BrandId == id && e.LastAccessed >= startDate && e.LastAccessed <= endDate)
                .AsNoTracking()
                .Select(e => new {
                    LastAccessed = e.LastAccessed,
                    EventId = e.EventId
                })
                .ToListAsync();

            var redeemVouchersTask = context.RedeemVouchers
                .Where(rd => rd.BrandId == id && rd.CreatedDate >= startDate && rd.CreatedDate <= endDate)
                .AsNoTracking()
                .Select(e => new {
                    EventId = e.EventId,
                    Value = e.Value
                })
                .ToListAsync();

            var eventsTask = context.Events
                .Where(e => e.BrandId == id && ((e.StartDate >= startDate && e.EndDate <= endDate) || e.EndDate >= startDate))
                .AsNoTracking()
                .Select(e => new {
                    Id = e.Id,
                    NoVoucher = e.NoVoucher,
                    RedeemVoucherCount = e.RedeemVoucherCount
                })
                .ToListAsync();

            await Task.WhenAll(playersTask, redeemVouchersTask, eventsTask);

            var players = playersTask.Result;
            var redeemVouchers = redeemVouchersTask.Result;
            var events = eventsTask.Result;

            var topEvents = all ? events
                .OrderByDescending(e => players.Count(p => p.EventId == e.Id))
                .Take(3)
                .Select(ev => new EventWithStatisticsVM {
                    PlayerData = new PlayerStatisticsVM {
                        Count = players.Count(p => p.EventId == ev.Id),
                        OnlineCount = players.Count(p => p.EventId == ev.Id && (DateTime.UtcNow - p.LastAccessed).TotalMinutes <= 5)
                    },
                    RedeemVoucherData = new RedeemVoucherStatisticsVM {
                        RedeemCount = ev.RedeemVoucherCount,
                        Total = ev.NoVoucher,
                        TotalValue = redeemVouchers.Where(v => v.EventId == ev.Id).Sum(v => v.Value)
                    }
                })
                .ToList() : null;

            return new BrandStatisticsVM {
                PlayerData = new PlayerStatisticsVM {
                    Count = players.Count,
                    OnlineCount = players.Count(p => (DateTime.UtcNow - p.LastAccessed).TotalMinutes <= 5)
                },
                RedeemVoucherData = new RedeemVoucherStatisticsVM {
                    Total = events.Sum(e => e.NoVoucher),
                    RedeemCount = events.Sum(e => e.RedeemVoucherCount),
                    TotalValue = redeemVouchers.Sum(v => v.Value)
                },
                TopEvents = new TopEventsStatisticsVM {
                    Events = topEvents
                },
                EventCount = events.Count
            };
        }

        public async Task<IEnumerable<RedeemVoucherVM>> GetBrandRedeemVoucher(int id) {
            var brand = await context.Brands.FindAsync(id);
            if (brand == null)
                throw new KeyNotFoundException();

            return await context.RedeemVouchers
                .Where(rd => rd.BrandId == id)
                .OrderByDescending(rd => rd.CreatedDate)
                .Select(e => new RedeemVoucherVM {
                    Id = e.Id,
                    CreatedDate = e.CreatedDate,
                    Value = e.Value
                })
                .Take(5)
                .ToListAsync();
        }
    }
}
