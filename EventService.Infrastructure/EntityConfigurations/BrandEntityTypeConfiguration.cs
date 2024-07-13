using EventService.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand> {
        public void Configure(EntityTypeBuilder<Brand> builder) {
            builder.ToTable("brand");
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id).HasName("brand_pkey");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            builder.Property(e => e.Address).HasColumnName("address");
            builder.Property(e => e.Field).HasColumnName("field");
            builder.Property(e => e.Gps).HasColumnName("gps");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Status).HasColumnName("status");
        }
    }
}
