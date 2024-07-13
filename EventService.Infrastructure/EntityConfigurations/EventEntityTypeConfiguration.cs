using EventService.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations
{
    public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event> {
        public void Configure(EntityTypeBuilder<Event> builder) {
            builder.ToTable("event");
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id).HasName("event_pkey");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            builder.Property(e => e.BrandId).HasColumnName("brand_id");
            builder.Property(e => e.EndDate).HasColumnName("end_date");
            builder.Property(e => e.GameId).HasColumnName("game_id");
            builder.Property(e => e.Image).HasColumnName("image");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.NoVoucher).HasColumnName("no_voucher");
            builder.Property(e => e.StartDate).HasColumnName("start_date");

            builder.HasOne(e => e.Brand)
                .WithMany(e => e.Events)
                .HasForeignKey(e => e.BrandId);
        }
    }
}
