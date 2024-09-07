using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InventoryService.API.Models;

namespace InventoryService.API.Infrastructure.EntityTypeConfigurations {
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.ToTable("user");
            builder.HasKey(e => e.Id).HasName("user_pkey");

            builder.HasMany(e => e.Vouchers)
                .WithOne();
            
            builder.HasMany(e => e.Items)
                .WithOne();

            builder.HasMany(e => e.ItemPieces)
                .WithOne();
        }
    }
}
