using System.Net;

namespace Cashflow.Domain.Exceptions.FinancialBoundaries;

public class AttempToAddMoreThanOneFinancialConstraintException : DomainException
{
    public AttempToAddMoreThanOneFinancialConstraintException() 
        : base("It is not possible to add more than one financial constraint to a category.", HttpStatusCode.BadRequest)
    { }
}