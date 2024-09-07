using InventoryService.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.API.Infrastructure.EntityTypeConfigurations {
    public class ItemPiecesEntityTypeConfiguration : IEntityTypeConfiguration<ItemPiece> {
        public void Configure(EntityTypeBuilder<ItemPiece> builder) {
            builder.ToTable("item_piece");
            builder.HasKey(e => e.Id).HasName("piece_pkey");
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            builder.Property(e => e.GameItemId).HasColumnName("item_id");
        }
    }
}
