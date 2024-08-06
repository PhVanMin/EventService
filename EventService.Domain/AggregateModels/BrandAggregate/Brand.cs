using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.AggregateModels.VoucherAggregate;
using EventService.Domain.Exceptions;
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

    public void Update(string name, string field, short status) {
        Name = name;
        Field = field;
        Status = status;
    }

    public void AddEvent(string name, string image, int noVoucher, DateTime start, DateTime end, int gameId) {
        var @event = _events.FirstOrDefault(e => e.Name == name);
        if (@event != null) {
            throw new EventDomainException($"Event with name \"{name}\" already exists.");
        }

        @event = new Event() {
            Name = name,
            Image = image,
            NoVoucher = noVoucher,
            StartDate = start,
            EndDate = end,
            GameId = gameId
        };

        _events.Add(@event);
    }
    public void AddVoucher(string code, string image, int value, string description, DateTime expireDate, int status) {
        var voucher = _vouchers.FirstOrDefault(v => v.Code == code);
        if (voucher != null)
            throw new EventDomainException("Voucher already exists.");

        voucher = new Voucher() {
            Code = code,
            Image = image,
            Value = value,
            Description = description,
            ExpireDate = expireDate,
            Status = status
        };

        _vouchers.Add(voucher);
    }
}
