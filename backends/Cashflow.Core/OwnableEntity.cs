namespace Cashflow.Core;

public abstract class OwnableEntity<TId, TOwnerId> : IEntity<TId>
    where TId : struct
    where TOwnerId : struct
{
    private LinkedList<IEvent> Events { get; } = new LinkedList<IEvent>();
    
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
    
    public void RaiseEvent(IEvent @event)
    {
        Events.AddLast(@event);
    }

    public IEnumerable<IEvent> Invoke() => Events;
}