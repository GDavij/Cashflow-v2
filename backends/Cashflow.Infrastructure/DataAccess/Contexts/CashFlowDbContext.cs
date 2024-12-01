using Cashflow.Core;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Cashflow.Infrastructure.DataAccess.Contexts;

public class CashFlowDbContext : DbContext, ICashflowDbContext
{
    private readonly IAuthenticatedUser _authenticatedUser;
    
    public DbSet<AccountType> AccountTypes { get; init; }
    public DbSet<AuditionEvent> AuditionEvents { get; init; }
    public DbSet<BankAccount> BankAccounts { get; init; }
    public DbSet<Category> Categories { get; init; }
    public DbSet<Recurrency> Recurrencies { get; init; }
    public DbSet<RecurrencyTime> RecurrenciesTime { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<Transaction> Transactions { get; init; }
    public DbSet<TransactionMethod> TransactionMethods { get; init; }
    public DbSet<User> Users { get; init; }

    public CashFlowDbContext(IAuthenticatedUser authenticatedUser)
    {
        _authenticatedUser = authenticatedUser;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashFlowDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<OwnableEntity<long, long>>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.OwnerId = _authenticatedUser.Id;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedAt = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = _authenticatedUser.Id;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}