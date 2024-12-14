namespace Cashflow.Core.Events.Tracing;

public class EntityActivatedEvent<T> : BaseEvent
    where T : OwnableEntity<T>
{
    private readonly T _entity;

    public EntityActivatedEvent(T entity) : base(false)
    {
        _entity = entity;
    }

    public override string Description() => $"Entity of type {typeof(T).FullName} identified by Id {_entity.Id} has been activated at {DateTime.UtcNow}";
}

