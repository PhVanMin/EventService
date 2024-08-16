using EventService.Domain.AggregateModels.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations
{
    public class EventVoucherConfiguration : IEntityTypeConfiguration<EventVoucher> {
        public void Configure(EntityTypeBuilder<EventVoucher> builder) {
            builder.ToTable("event_voucher");

            builder.HasKey(ve => new { ve.EventId, ve.VoucherId });

            builder.HasOne(ve => ve.Event)
                .WithMany(e => e.Vouchers)
                .HasForeignKey(ve => ve.EventId);

            builder.HasOne(ve => ve.Voucher)
                .WithMany()
                .HasForeignKey(ve => ve.VoucherId);
        }
    }
}
