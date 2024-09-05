using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.AggregateModels.PlayerAggregate;
using EventService.Domain.AggregateModels.VoucherAggregate;
using EventService.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EventService.Infrastructure;

public class EventDbContext : DbContext {
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<EventVoucher> EventVoucher { get; set; }
    public virtual DbSet<EventPlayer> EventPlayer { get; set; }
    public virtual DbSet<Player> Players { get; set; }
    public virtual DbSet<Voucher> Vouchers { get; set; }
    public virtual DbSet<RedeemVoucher> RedeemVouchers { get; set; }

    private IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;
    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;

    public EventDbContext(DbContextOptions<EventDbContext> options, IMediator mediator)
        : base(options) {
        _mediator = mediator;
        Database.Migrate();
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync() {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction) {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        } catch {
            RollbackTransaction();
            throw;
        } finally {
            if (_currentTransaction != null) {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction() {
        try {
            _currentTransaction?.Rollback();
        } finally {
            if (_currentTransaction != null) {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default) {
        await _mediator.DispatchDomainEventsAsync(this);
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

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
        modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EventEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EventVoucherConfiguration());
        modelBuilder.ApplyConfiguration(new EventPlayerConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new VoucherEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RedeemVoucherEntityTypeConfiguration());
    }
}
