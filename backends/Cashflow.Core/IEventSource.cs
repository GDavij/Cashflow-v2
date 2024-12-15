namespace Cashflow.Core;


public interface IEventSource
{
    void RaiseEvent(IEvent @event);
    IEnumerable<IEvent> Invoke();
}
