using EventService.Domain.AggregateModels.GameAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game> {
        public void Configure(EntityTypeBuilder<Game> builder) {
            builder.ToTable("game");
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(e => e.Id).HasName("game_pkey");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Image).HasColumnName("image");
            builder.Property(e => e.Name).HasColumnName("name");
        }
    }
}
