using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.AggregateModels.VoucherAggregate;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace EventService.Domain.AggregateModels.BrandAggregate;

public class Brand : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;

    public string Field { get; set; } = null!;

    [Required]
    public Location Location { get; set; } = null!;

    public int Status { get; set; }

    private List<Event> _events;
    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

    private List<Voucher> _vouchers;
    public IReadOnlyCollection<Voucher> Vouchers => _vouchers.AsReadOnly();

    public Brand() {
        _events = new List<Event>();
        _vouchers = new List<Voucher>();
    }

    public void Update(string? name, string? field, short? status) {
        if (name != null) Name = name;
        if (field != null) Field = field;
        if (status != null) Status = status.Value;
    }

    public Event AddEvent(string name, string image, int noVoucher, DateTime start, DateTime end, Guid? gameId, List<int>? voucherIds) {
        var @event = new Event(name, image, noVoucher, start, end, gameId);

        if (voucherIds != null) {
            foreach (var voucherId in voucherIds) {
                @event.AddVoucher(voucherId);
            }
        }

        _events.Add(@event);

        return @event;
    }
    public Voucher AddVoucher(string image, int value, string? description, int expireDate, int status) {
        var voucher = new Voucher() {
            Image = image,
            Value = value,
            Description = description,
            ExpireDate = expireDate,
            Status = status,
            CreatedDate = DateTime.UtcNow,
        };

        _vouchers.Add(voucher);
        return voucher;
    }
}
