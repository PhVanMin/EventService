using EventService.Domain.AggregateModels.BrandAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations
{
    public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand> {
        public void Configure(EntityTypeBuilder<Brand> builder) {
            builder.ToTable("brand");
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id).HasName("brand_pkey");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            builder.OwnsOne(
                e => e.Location, 
                lo => {
                    lo.Property(l => l.Address).HasColumnName("address");
                    lo.Property(l => l.Gps).HasColumnName("gps");
                }
            );

            builder.Property(e => e.Field).HasColumnName("field");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Status).HasColumnName("status");
        }
    }
}
