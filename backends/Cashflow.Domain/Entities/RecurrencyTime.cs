using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class RecurrencyTime : ValueObject<short>
{
    public required string Name { get; init; }
}