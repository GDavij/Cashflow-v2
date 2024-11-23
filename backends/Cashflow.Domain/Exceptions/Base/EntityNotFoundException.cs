using System.Net;

namespace Cashflow.Domain.Exceptions;

public class EntityNotFoundException<T> : DomainException
{
    public EntityNotFoundException()
        : base($"Could not found {nameof(T)}.", HttpStatusCode.NotFound)
    { }
    
    public EntityNotFoundException(string entityName)
        : base($"Could not found {entityName}.", HttpStatusCode.NotFound)
    { }
}