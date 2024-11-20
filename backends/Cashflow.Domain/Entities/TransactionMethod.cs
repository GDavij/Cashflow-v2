using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class TransactionMethod : ValueObject<short>
{
    public string Name { get; init; }
}