namespace Cashflow.Core;

public interface IEntity<TId>
    where TId : struct
{
    TId Id { get; init; }

    bool Active { get; }
    bool Deleted { get; }
    long? OwnerId { get; set; }
    DateTime CreatedAt { get; set; }
    long? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }

    void Deactivate();

    void Activate();

    void Delete();
    
    void RaiseEvent(IEvent @event);
}