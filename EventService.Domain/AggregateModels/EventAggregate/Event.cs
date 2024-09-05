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
    public Guid? GameId { get; set; }

    private List<EventVoucher> _vouchers = [];
    public IReadOnlyCollection<EventVoucher> Vouchers => _vouchers.AsReadOnly();

    private List<EventPlayer> _players = [];
    public IReadOnlyCollection<EventPlayer> Players => _players.AsReadOnly();

    public Event() {

    }

    public Event(string name, string image, int noVoucher, DateTime start, DateTime end, Guid? gameId) {
        if (start > end)
            throw new EventDomainException("Invalid end date");

        if (start < DateTime.UtcNow)
            throw new EventDomainException("Invalid start date");

        Name = name;
        Image = image;
        NoVoucher = noVoucher;
        StartDate = start.ToUniversalTime();
        EndDate = end.ToUniversalTime();
        GameId = gameId;

        if (gameId != null) {
            AddDomainEvent(new EventStartDomainEvent(this));
            AddDomainEvent(new EventVoucherUsedUpOrEndDomainEvent(this));
        }
    }

    public void Update(string? name, string? image, int? noVoucher, DateTime? start, DateTime? end, Guid? gameId) {
        if (start > end)
            throw new EventDomainException("Invalid end date");

        if (start < DateTime.UtcNow)
            throw new EventDomainException("Invalid start date");

        if (name != null) Name = name;
        if (image != null) Image = image;
        if (noVoucher != null) NoVoucher = noVoucher.Value;
        if (start != null) StartDate = start.Value.ToUniversalTime();
        if (end != null) EndDate = end.Value.ToUniversalTime();

        if (gameId != null && gameId.Value != GameId) {
            AddDomainEvent(new EventStartDomainEvent(this));
            AddDomainEvent(new EventVoucherUsedUpOrEndDomainEvent(this));
        } else if (GameId != null && start != StartDate) {
            AddDomainEvent(new EventStartDomainEvent(this));
        } else if (GameId != null && end != EndDate) {
            AddDomainEvent(new EventVoucherUsedUpOrEndDomainEvent(this));
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
            eventPlayer.LastAccessed = DateTime.UtcNow;
        else {
            var player = new Player(name, email);
            _players.Add(new EventPlayer { EventId = Id, Player = player, LastAccessed = DateTime.UtcNow });
        }
    }

    public void IncrementRedeemVoucherCount() {
        if (GameId == null)
            throw new EventDomainException("Event doesn't register a game.");

        if (RedeemVoucherCount >= NoVoucher)
            throw new EventDomainException("Vouchers are used up.");

        if (RedeemVoucherCount == NoVoucher - 1)
            AddDomainEvent(new EventVoucherUsedUpOrEndDomainEvent(this));

        RedeemVoucherCount += 1;
    }
}
