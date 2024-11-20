using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cashflow.Infrastructure.DataAccess.Contexts;

public class CashFlowDbContext : DbContext, ICashflowDbContext
{
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashFlowDbContext).Assembly);
    }
}