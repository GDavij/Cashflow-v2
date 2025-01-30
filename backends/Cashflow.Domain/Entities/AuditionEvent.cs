using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class AuditionEvent : ValueObject<long>, IEvent
{
    public required string @Event { get; init; }
    public string? IpAddress { get; init; }
    public DateTime OccuredAt { get; init; }
    public Guid TraceIdentifier { get; set; }
    public bool PrivateEvent { get; init; }
    public string? UserAgent { get; init; }
    public User? User { get; init; }
    public long UserId { get; init; }
    public bool Private { get; init; }

    public void BindToTrace(Guid traceId)
    {
        TraceIdentifier = traceId;
    }
}
