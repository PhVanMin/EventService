using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InventoryService.API.Models;

namespace InventoryService.API.Infrastructure.EntityTypeConfigurations {
    public class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item> {
        public void Configure(EntityTypeBuilder<Item> builder) {
            builder.ToTable("item");
            builder.HasKey(e => e.Id).HasName("item_pkey");
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            builder.Property(e => e.GameItemId).HasColumnName("gameItem_id");
            builder.Property(e => e.EventId).HasColumnName("event_id");
        }
    }
}
