namespace Cashflow.Core;

public abstract class ValueObject<TId> : IEntity<TId>
    where TId : struct
{
    public TId Id { get; init; }
}