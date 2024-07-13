﻿using EventService.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    public class VoucherEntityTypeConfiguration : IEntityTypeConfiguration<Voucher> {
        public void Configure(EntityTypeBuilder<Voucher> builder) {
            builder.ToTable("voucher");
            builder.HasKey(e => e.Id).HasName("voucher_pkey");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            builder.Property(e => e.Code).HasColumnName("code");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.EventId).HasColumnName("event_id");
            builder.Property(e => e.ExpireDate).HasColumnName("expire_date");
            builder.Property(e => e.Image).HasColumnName("image");
            builder.Property(e => e.Status).HasColumnName("status");
            builder.Property(e => e.Value).HasColumnName("value");

            builder.HasOne(e => e.Event)
                    .WithMany(e => e.Vouchers)
                    .HasForeignKey(e => e.EventId)
                    .IsRequired();
        }
    }
}
