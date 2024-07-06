using EventService.Domain.AggregatesModel.EventAggregate;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregatesModel.VoucherAggregate;
public class Voucher : Entity, IAggregateRoot {
    public string Code { get; set; } = null!;

    public string Image { get; set; } = null!;

    public long Value { get; set; }

    public string Description { get; set; } = null!;

    public DateOnly ExpireDate { get; set; }

    public short Status { get; set; }

    public long EventId { get; set; }

    public virtual Event Event { get; set; } = null!;
}
