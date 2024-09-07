using InventoryService.API.Infrastructure.EntityTypeConfigurations;
using InventoryService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.API.Infrastructure {
    public class InventoryDbContext : DbContext {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemPiece> ItemPieces { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherEntityTypeConfiguration());
        }
    }
}
