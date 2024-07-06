using EventService.Domain.AggregatesModel.BrandAggregate;
using EventService.Domain.AggregatesModel.VoucherAggregate;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregatesModel.EventAggregate;

public class Event : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public long NoVoucher { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public long BrandId { get; set; }

    public long? GameId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
