using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class RecurrencyTime : ValueObject<short>
{
    public string Name { get; init; }
}