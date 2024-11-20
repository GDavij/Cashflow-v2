using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class AuditionEvents : ValueObject<long>
{
    public string Event { get; init; }
    public string? IpAddress { get; init; }
    public DateTime OccuredAt { get; init; }
    public bool PrivateEvent { get; init; }
    public Guid TraceIdentitifier { get; init; }
    public string? UserAgent { get; init; }
    public long UserId { get; init; }
}