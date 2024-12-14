using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class Recurrency : OwnableEntity<Recurrency>
{
    public short RecurrencyTimeId { get; init; }
    public RecurrencyTime RecurrencyTime { get; init; }
    public int Times { get; init; }
    public decimal TransactionValue { get; init; }
    public short TransactionMethodId { get; init; }
    public TransactionMethod TransactionMethod { get; init; }
}