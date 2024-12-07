using System.Net;

namespace Cashflow.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected HttpStatusCode StatusCode { get;  }

    protected DomainException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}