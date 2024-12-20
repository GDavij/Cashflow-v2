using System.Net;

namespace Cashflow.Domain.Exceptions.FinancialDistribution;

public class AttemptToDuplicateBankAccountNameException : DomainException
{
    public AttemptToDuplicateBankAccountNameException(string bankAccountName)
        : base($"It is not possible to duplicate a Bank Account named {bankAccountName}.", HttpStatusCode.Conflict)
    { }
}
