using Cashflow.Core;

namespace Cashflow.Domain.Events;

public abstract class BaseEvent : IEvent
{
    public long Id { get; init; }
    public Guid TraceIdentifier { get; set; }
    public DateTime OccuredAt { get; init; } = DateTime.UtcNow;
    public bool Private { get; init; }

    public BaseEvent(bool @private)
    {
        Private = @private;
    }

    public void BindToTrace(Guid traceId) => TraceIdentifier = traceId;
    public abstract string Description();
}

