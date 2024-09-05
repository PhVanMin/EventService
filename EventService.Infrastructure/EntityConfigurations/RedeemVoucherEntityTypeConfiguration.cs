using EventService.Domain.AggregateModels.VoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    public class RedeemVoucherEntityTypeConfiguration : IEntityTypeConfiguration<RedeemVoucher> {
        public void Configure(EntityTypeBuilder<RedeemVoucher> builder) {
            builder.ToTable("redeem");
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id).HasName("redeem_pkey");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            builder.Property(e => e.BrandId).HasColumnName("brand_id");
            builder.Property(e => e.EventId).HasColumnName("event_id");
            builder.Property(e => e.CreatedDate).HasColumnName("date");
            builder.Property(e => e.Value).HasColumnName("value");
        }
    }
}
