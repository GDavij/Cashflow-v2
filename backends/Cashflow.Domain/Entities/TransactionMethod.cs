using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class TransactionMethod : ValueObject<long>
{
    public string Name { get; init; }
}