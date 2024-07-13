using EventService.Domain.Model;
using EventService.Infrastructure.EntityConfigurations;
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
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("database"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new BrandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EventEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new VoucherEntityTypeConfiguration());
    }
}
