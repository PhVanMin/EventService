using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.AggregateModels.PlayerAggregate;
using EventService.Domain.Events;
using EventService.Domain.Exceptions;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.EventAggregate;

public class Event : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public int NoVoucher { get; set; }
    public int RedeemVoucherCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    public int? GameId { get; set; }

    private List<EventVoucher> _vouchers = [];
    public IReadOnlyCollection<EventVoucher> Vouchers => _vouchers.AsReadOnly();

    private List<EventPlayer> _players = [];
    public IReadOnlyCollection<EventPlayer> Players => _players.AsReadOnly();

    public Event() {

    }

    public Event(string name, string image, int noVoucher, DateTime start, DateTime end, int? gameId, List<int> voucherIds) {
        Name = name;
        Image = image;
        NoVoucher = noVoucher;
        StartDate = start;
        EndDate = end;
        GameId = gameId;

        if (gameId != null) {
            AddDomainEvent(new EventGameRegisteredOrUpdateDomainEvent(this, gameId.Value));
        }

        AddDomainEvent(new EventStartDomainEvent(this));
    }

    public void Update(string name, string image, int noVoucher, DateTime start, DateTime end, int? gameId) {
        Name = name;
        Image = image;
        NoVoucher = noVoucher;
        StartDate = start;
        EndDate = end;

        if (gameId != null && gameId != GameId) {
            GameId = gameId;
            AddDomainEvent(new EventGameRegisteredOrUpdateDomainEvent(this, gameId.Value));
        }

        _vouchers.Clear();
    }

    public void AddVoucher(int voucherId) {
        var newVoucher = _vouchers.FirstOrDefault(v => v.VoucherId == voucherId);
        if (newVoucher != null)
            throw new EventDomainException("Voucher already exists.");

        _vouchers.Add(new EventVoucher {
            EventId = Id,
            VoucherId = voucherId,
        });
    }

    public void AddPlayer(string name, string email) {
        var eventPlayer = _players.FirstOrDefault(v => v.Player.Email == email);
        if (eventPlayer != null)
            eventPlayer.Player.LastAccessed = DateTime.UtcNow;
        else {
            var player = new Player(name, email);
            _players.Add(new EventPlayer { EventId = Id, Player = player });
        }
    }

    public void IncrementRedeemVoucherCount() {
        if (RedeemVoucherCount >= NoVoucher)
            throw new EventDomainException("Vouchers are used up.");

        if (RedeemVoucherCount == NoVoucher - 1)
            AddDomainEvent(new EventVoucherUsedUpOrEndDomainEvent(Id));

        RedeemVoucherCount += 1;
    }
}
