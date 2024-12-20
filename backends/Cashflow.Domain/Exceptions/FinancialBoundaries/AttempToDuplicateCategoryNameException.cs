using System.Net;

namespace Cashflow.Domain.Exceptions.FinancialBoundaries;

public class AttempToDuplicateCategoryNameException : DomainException
{
    public AttempToDuplicateCategoryNameException(string categoryName) 
        : base($"It is not possible to duplicate a category named {categoryName}.", HttpStatusCode.Conflict)
    { }
}