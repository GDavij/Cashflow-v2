namespace Cashflow.Core.Events.Tracing;

public class EntityCreatedEvent<T> : BaseEvent
    where T : OwnableEntity<T>
{
    private readonly T _entity;

    public EntityCreatedEvent(T entity) : base(false)
    {
        _entity = entity;
    }

    public override string Description() => $"Entity of type {typeof(T).FullName} identified by Id {_entity.Id} has been created at {DateTime.UtcNow}";
}
