using System.Net;

namespace Cashflow.Domain.Exceptions.FinancialBoundaries;

public class AttempToDupplicateCategoryNameException : DomainException
{
    public AttempToDupplicateCategoryNameException(string categoryName) 
        : base($"It is not possible to duplicate a category named {categoryName}", HttpStatusCode.Conflict)
    { }
}