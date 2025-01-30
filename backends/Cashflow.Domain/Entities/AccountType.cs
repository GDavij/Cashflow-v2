using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class AccountType : ValueObject<short>
{
    public required string Name { get; init; }
    public ICollection<BankAccount> BankAccounts { get; init; } = [];
}