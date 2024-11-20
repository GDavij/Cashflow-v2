using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cashflow.Domain.Abstractions.DataAccess;

public interface ICashflowDbContext
{
    DbSet<AccountType> AccountTypes { get; init; }
    DbSet<AuditionEvent> AuditionEvents { get; init; }
    DbSet<BankAccount> BankAccounts { get; init; }
    DbSet<Category> Categories { get; init; }
    DbSet<Recurrency> Recurrencies { get; init; }
    DbSet<RecurrencyTime> RecurrenciesTime { get; init; }
    DbSet<Role> Roles { get; init; }
    DbSet<Transaction> Transactions { get; init; }
    DbSet<TransactionMethod> TransactionMethods { get; init; }
    DbSet<User> Users { get; init; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}