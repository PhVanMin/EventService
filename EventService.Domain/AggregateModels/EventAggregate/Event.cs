using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.AggregateModels.GameAggregate;
using EventService.Domain.AggregateModels.PlayerAggregate;
using EventService.Domain.AggregateModels.VoucherAggregate;
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
    public Game? Game { get; set; }

    private List<EventVoucher> _vouchers;
    public IReadOnlyCollection<EventVoucher> Vouchers => _vouchers.AsReadOnly();

    private List<EventPlayer> _players;
    public IReadOnlyCollection<EventPlayer> Players => _players.AsReadOnly();

    private List<RedeemVoucher> _redeemVouchers;
    public IReadOnlyCollection<RedeemVoucher> RedeemVouchers => _redeemVouchers.AsReadOnly();

    public Event() {
        _redeemVouchers = new List<RedeemVoucher>();
        _vouchers = new List<EventVoucher>();
        _players = new List<EventPlayer>();
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
            VoucherId = voucherId,
        });
    }

    public RedeemVoucher AddRedeemVoucher(int playerId, int voucherId) {
        if (RedeemVoucherCount >= NoVoucher) {
            throw new EventDomainException($"Cannot create more than {NoVoucher} vouchers.");
        }

        RedeemVoucherCount += 1;
        if (RedeemVoucherCount == NoVoucher) {
            AddDomainEvent(new EventVoucherUsedUpDomainEvent(Id));
        }

        var eventVoucher = _vouchers.FirstOrDefault(v => v.VoucherId == voucherId);
        if (eventVoucher == null) {
            throw new EventDomainException("Voucher does not exist.");
        }

        var redeemVoucher = eventVoucher.Voucher.GenerateRedeemVoucher(playerId, Id);
        _redeemVouchers.Add(redeemVoucher);
        return redeemVoucher;
    }

    public void RedeemVoucher(int redeemVoucherId, string code) {
        var redeemVoucher = _redeemVouchers.FirstOrDefault(rv => rv.Id == redeemVoucherId);
        if (redeemVoucher == null) {
            throw new EventDomainException("Voucher does not exists.");
        }

        redeemVoucher.VerifyAndRedeem(code);
    }
    
    public void AddPlayer(string name, string email) {
        var eventPlayer = _players.FirstOrDefault(v => v.Player.Email == email);
        if (eventPlayer != null)
            eventPlayer.Player.LastAccessed = DateTime.Now;
        else {
            var player = new Player(name, email);
            _players.Add(new EventPlayer { EventId = Id, Player = player });
        }
    }
}
