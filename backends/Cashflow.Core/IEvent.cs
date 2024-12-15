using MediatR;

namespace Cashflow.Core;

// Allow MediatR to handle events (coupling)
public interface IEvent : INotification
{
    public long Id { get; init; }
    public Guid TraceIdentifier { get; set; }
    public DateTime OccuredAt { get; init; }
    public bool Private { get; init; }

    public void BindToTrace(Guid traceId);
}