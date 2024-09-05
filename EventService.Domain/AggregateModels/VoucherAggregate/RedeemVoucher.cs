using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.VoucherAggregate {
    public class RedeemVoucher : Entity {
        public int BrandId { get; set; }
        public int EventId { get; set; }
        public int Value { get; set; }
        public DateTime CreatedDate { get; set; }

        public RedeemVoucher(int brandId, int eventId, int value)
        {
            BrandId = brandId;
            EventId = eventId;
            Value = value;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
