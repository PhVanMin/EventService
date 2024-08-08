using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.VoucherAggregate;
public class Voucher : Entity, IAggregateRoot {
    public string Code { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int Value { get; set; }

    public string Description { get; set; } = null!;

    public int ExpireDate { get; set; }

    public int Status { get; set; }

    public int BrandId { get; set; }

    public Brand Brand { get; set; } = null!;

    private List<EventVoucher> _events;
    public IReadOnlyCollection<EventVoucher> EventVouchers => _events.AsReadOnly();

    public Voucher() {
        _events = new List<EventVoucher>();
    }

    public void Update(string image, int value, string description, int expireDate, int status) {
        Image = image;
        Value = value;
        Description = description;
        ExpireDate = expireDate;
        Status = status;
    }
}
