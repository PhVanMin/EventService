using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.AggregateModels.VoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    public class RedeemVoucherEntityTypeConfiguration : IEntityTypeConfiguration<RedeemVoucher> {
        public void Configure(EntityTypeBuilder<RedeemVoucher> builder) {
            builder.ToTable("redeem_voucher");
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id).HasName("redeem_voucher_pkey");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            builder.Property(e => e.RedeemCode).HasColumnName("code");
            builder.Property(e => e.ExpireDate).HasColumnName("expire_date");
            builder.Property(e => e.RedeemTime).HasColumnName("redeem_time");

            builder.HasOne(e => e.Player)
                .WithMany()
                .HasForeignKey(e => e.PlayerId);

            builder.HasOne(e => e.BaseVoucher)
                .WithMany()
                .HasForeignKey(e => e.BaseVoucherId);

            builder.HasOne(e => e.Event)
                .WithMany(e => e.RedeemVouchers)
                .HasForeignKey(e => e.EventId);
        }
    }
}
