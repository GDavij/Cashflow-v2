namespace Cashflow.Core.Events.Tracing;

public class EntityDeletedEvent<T> : BaseEvent
    where T : OwnableEntity<T>
{
    private readonly T _entity;

    public EntityDeletedEvent(T entity) : base(false)
    {
        _entity = entity;
    }

    public override string Description() => $"Entity of type {typeof(T).FullName} identified by Id {_entity.Id} has been deleted at {DateTime.UtcNow}";
}
