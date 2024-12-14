namespace Cashflow.Core.Events.Tracing;

public class EntityDeactivatedEvent<T> : BaseEvent
    where T : OwnableEntity<T>
{
    private readonly T _entity;

    public EntityDeactivatedEvent(T entity) : base(false)
    {
        _entity = entity;
    }

    public override string Description()
    {
        throw new NotImplementedException();
    }
}
