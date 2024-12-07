using Cashflow.Core;
using Cashflow.Domain.Events.FinancialDistribution;

namespace Cashflow.Domain.Entities;

public class BankAccount : OwnableEntity<long, long>
{
    public short AccountTypeId { get; init; }
    public AccountType AccountType { get; init; }
    public decimal CurrentValue { get; init; }
    public string Name { get; init; }

    public ICollection<Transaction> Transactions { get; init; } = new List<Transaction>();

    public BankAccount(short accountTypeId, string name)
    {
        AccountTypeId = accountTypeId;
        Name = name;
        CurrentValue = 0.0M;
        
        RaiseEvent(new BankAccountCreatedEvent(this));
    }

    public BankAccount(short accountTypeId, string name, decimal currentValue)
    {
        AccountTypeId = accountTypeId;
        Name = name;
        CurrentValue = currentValue;
        
        RaiseEvent(new BankAccountCreatedEvent(this));
    }
    
}