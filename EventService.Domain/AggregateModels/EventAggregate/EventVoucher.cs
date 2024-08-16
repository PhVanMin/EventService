using EventService.Domain.AggregateModels.VoucherAggregate;

namespace EventService.Domain.AggregateModels.EventAggregate {
    public class EventVoucher {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; } = null!;
    }
}
