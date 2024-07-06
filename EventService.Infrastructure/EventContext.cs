using EventService.Domain.AggregatesModel.BrandAggregate;
using EventService.Domain.AggregatesModel.EventAggregate;
using EventService.Domain.AggregatesModel.VoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventService.Infrastructure;

public class EventContext : DbContext {
    public EventContext() {
    }

    public EventContext(DbContextOptions<EventContext> options)
        : base(options) {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("EventDB"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Brand>(entity => {
            entity.HasKey(e => e.Id).HasName("brand_pkey");

            entity.ToTable("brand");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Field).HasColumnName("field");
            entity.Property(e => e.Gps).HasColumnName("gps");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Event>(entity => {
            entity.HasKey(e => e.Id).HasName("event_pkey");

            entity.ToTable("event");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NoVoucher).HasColumnName("no_voucher");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Brand).WithMany(p => p.Events)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("event_brand_id_fkey");
        });

        modelBuilder.Entity<Voucher>(entity => {
            entity.HasKey(e => e.Id).HasName("voucher_pkey");

            entity.ToTable("voucher");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.ExpireDate).HasColumnName("expire_date");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Event).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("voucher_event_id_fkey");
        });
    }
}
