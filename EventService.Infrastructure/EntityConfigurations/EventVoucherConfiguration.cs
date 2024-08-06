using EventService.Domain.AggregateModels.BrandAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventService.Domain.AggregateModels;

namespace EventService.Infrastructure.EntityConfigurations {
    public class EventVoucherConfiguration : IEntityTypeConfiguration<EventVoucher> {
        public void Configure(EntityTypeBuilder<EventVoucher> builder) {
            builder.ToTable("brand_voucher");

            builder.HasKey(ve => new { ve.EventId, ve.VoucherId });

            builder.HasOne(ve => ve.Event)
                .WithMany(e => e.EventVouchers)
                .HasForeignKey(ve => ve.EventId);

            builder.HasOne(ve => ve.Voucher)
                .WithMany(v => v.EventVouchers)
                .HasForeignKey(ve => ve.VoucherId);
        }
    }
}
