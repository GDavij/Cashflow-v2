namespace Cashflow.Core;

public abstract class OwnableEntity<TId, TOwnerId> : IEntity<TId>
    where TId : struct
    where TOwnerId : struct
{
    public TId Id { get; init; }
    public bool Active { get; private set; } = false;
    public bool Deleted { get; private set; } = false;
    public TOwnerId? OwnerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public TOwnerId? LastModifiedBy { get; init; }
    public DateTime? LastModifiedAt { get; init; }

    public void Deactivate() => Active = false;
    public void Activate() => Active = true;
    public void Delete() => Deleted = true;
}