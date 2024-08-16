using EventService.Domain.AggregateModels.PlayerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player> {
        public void Configure(EntityTypeBuilder<Player> builder) {
            builder.ToTable("player");
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id).HasName("player_pkey");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Email).HasColumnName("email");
            builder.Property(e => e.LastAccessed).HasColumnName("last_accessed");
        }
    }
}
