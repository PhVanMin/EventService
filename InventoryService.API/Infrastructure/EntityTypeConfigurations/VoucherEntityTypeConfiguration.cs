using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InventoryService.API.Models;

namespace InventoryService.API.Infrastructure.EntityTypeConfigurations {
    public class VoucherEntityTypeConfiguration : IEntityTypeConfiguration<Voucher> {
        public void Configure(EntityTypeBuilder<Voucher> builder) {
            builder.ToTable("voucher");
            builder.HasKey(e => e.Id).HasName("voucher_pkey");

            builder.Property(e => e.EventId).HasColumnName("event_id");
            builder.Property(e => e.VoucherId).HasColumnName("voucher_id");
            builder.Property(e => e.Code).HasColumnName("code");
            builder.Property(e => e.ExpireDate).HasColumnName("expire_date");
        }
    }
}
