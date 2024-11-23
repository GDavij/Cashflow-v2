namespace Cashflow.Core;

public interface IEvent
{
    public long Id { get; init; }
    public Guid TraceIdentifier { get; set; }
    public DateTime OccuredAt { get; init; }
}