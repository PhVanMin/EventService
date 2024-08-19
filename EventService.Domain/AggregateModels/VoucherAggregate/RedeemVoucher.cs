using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.AggregateModels.PlayerAggregate;
using EventService.Domain.Exceptions;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.VoucherAggregate {
    public class RedeemVoucher : Entity {
        public string RedeemCode { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
        public DateTime RedeemTime { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int BaseVoucherId { get; set; }
        public Voucher BaseVoucher { get; set; } = null!;
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public RedeemVoucher(int playerId, int baseVoucherId, int eventId, string redeemCode, DateTime expireDate) {
            RedeemCode = redeemCode;
            ExpireDate = expireDate;
            PlayerId = playerId;
            BaseVoucherId = baseVoucherId;
            EventId = eventId;
        }

        public void VerifyAndRedeem(string code) {
            if (code != RedeemCode) {
                throw new EventDomainException("Voucher code is not correct.");
            }

            if (RedeemTime != default(DateTime)) {
                throw new EventDomainException("Voucher already redeemed.");
            }

            if (DateTime.UtcNow > ExpireDate) {
                throw new EventDomainException("Voucher expired.");
            }

            RedeemTime = DateTime.UtcNow;
        }
    }
}
