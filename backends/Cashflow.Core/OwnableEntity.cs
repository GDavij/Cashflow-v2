using Cashflow.Core.Events.Tracing;

namespace Cashflow.Core;

public abstract class OwnableEntity<TEntity> : IEntity<long>
    where TEntity : OwnableEntity<TEntity>
{
    private LinkedList<IEvent> Events { get; } = new LinkedList<IEvent>();
    
    public long Id { get; init; }
    public bool Active { get; private set; } = false;
    public bool Deleted { get; private set; } = false;
    public long? OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public long? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }

    public OwnableEntity()
    {
        // Evitates raising more than one Created Event when pulling from the database
        if (Id == default)
        {
            RaiseEvent(new EntityCreatedEvent<TEntity>((TEntity)this));
        }
    }

    public void Deactivate()
    {
        Active = false;
        RaiseEvent(new EntityDeactivatedEvent<TEntity>((TEntity)this));
    }
    
    public void Activate()
    {
        Active = true;
        RaiseEvent(new EntityActivatedEvent<TEntity>((TEntity)this));
    }

    public void Delete()
    {
        Deleted = true;
        RaiseEvent(new EntityDeletedEvent<TEntity>((TEntity)this));
    }
    
    public void RaiseEvent(IEvent @event)
    {
        Events.AddLast(@event);
    }
    

    public IEnumerable<IEvent> Invoke() => Events;
}