using Cashflow.Core;

namespace Cashflow.Domain.Abstractions.EventHandling;

public interface IEventMediator
{
    Task NotifyAsync(IEvent @event);
}
