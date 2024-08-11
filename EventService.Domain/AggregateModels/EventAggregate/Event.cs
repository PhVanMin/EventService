using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.Exceptions;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;
using MediatR;

namespace EventService.Domain.AggregateModels.EventAggregate;

public class Event : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int NoVoucher { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int BrandId { get; set; }

    public int? GameId { get; set; }

    public Brand Brand { get; set; } = null!;

    private List<EventVoucher> _vouchers;
    public IReadOnlyCollection<EventVoucher> Vouchers => _vouchers.AsReadOnly();

    public Event() {
        _vouchers = new List<EventVoucher>();
    }

    public void Update(string name, string image, int noVoucher, DateTime start, DateTime end, int gameId, List<int> voucherIds) {
        Name = name;
        Image = image;
        NoVoucher = noVoucher;
        StartDate = start;
        EndDate = end;
        GameId = gameId;

        _vouchers.Clear();
        foreach (var voucherId in voucherIds)
            AddVoucher(voucherId);
    }

    public void AddVoucher(int voucherId) {
        var newVoucher = _vouchers.FirstOrDefault(v => v.VoucherId == voucherId);
        if (newVoucher != null)
            throw new EventDomainException("Voucher already exists.");

        _vouchers.Add(new EventVoucher {
            EventId = Id,
            VoucherId = voucherId
        });
    }
}
