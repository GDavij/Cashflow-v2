using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class BankAccount : OwnableEntity<long, long>
{
    public short AccountTypeId { get; init; }
    public AccountType AccountType { get; init; }
    public decimal CurrentValue { get; init; }
    public string Name { get; init; }
    
    public ICollection<Transaction> Transactions { get; init; }
}