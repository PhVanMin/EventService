using EventService.Domain.AggregateModels.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    public class EventPlayerConfiguration : IEntityTypeConfiguration<EventPlayer> {
        public void Configure(EntityTypeBuilder<EventPlayer> builder) {
            builder.ToTable("event_player");

            builder.HasKey(pe => new { pe.EventId, pe.PlayerId });
            builder.Property(e => e.LastAccessed).HasColumnName("last_accessed");

            builder.HasOne(pe => pe.Event)
                .WithMany(e => e.Players)
                .HasForeignKey(pe => pe.EventId);

            builder.HasOne(pe => pe.Player)
                .WithMany(p => p.Events)
                .HasForeignKey(pe => pe.PlayerId);
        }
    }
}
