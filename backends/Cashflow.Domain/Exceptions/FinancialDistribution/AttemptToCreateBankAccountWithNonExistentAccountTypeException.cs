using System.Net;

namespace Cashflow.Domain.Exceptions.FinancialDistribution;
public class AttemptToCreateBankAccountWithNonExistentAccountTypeException : DomainException
{
    public AttemptToCreateBankAccountWithNonExistentAccountTypeException(short requestAccountType)
        : base($"Attempt to create a bank account with non existent account type {requestAccountType}.", HttpStatusCode.BadRequest)
    { }
}
