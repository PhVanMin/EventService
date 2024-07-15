using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.AggregateModels.VoucherAggregate;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace EventService.Domain.AggregateModels.EventAggregate;

public class Event : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int NoVoucher { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int BrandId { get; set; }

    public int? GameId { get; set; }

    [JsonIgnore]
    public Brand Brand { get; set; } = null!;

    private List<Voucher> _vouchers;
    public IReadOnlyCollection<Voucher> Vouchers => _vouchers.AsReadOnly();

    public Event() {
        _vouchers = new List<Voucher>();
    }
}
