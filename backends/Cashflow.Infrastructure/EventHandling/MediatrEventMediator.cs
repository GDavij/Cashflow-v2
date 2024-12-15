using Cashflow.Core;
using Cashflow.Domain.Abstractions.EventHandling;
using MediatR;

namespace Cashflow.Infrastructure.EventHandling;

public class MediatrEventMediator : IEventMediator
{
    private readonly IMediator _mediator;

    public MediatrEventMediator(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task NotifyAsync(IEvent @event)
    {
        return _mediator.Publish(@event);
    }
}
