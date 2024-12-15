using Cashflow.Core;
using Cashflow.Domain.Entities;

namespace Cashflow.Core.Events.FinancialDistribution;

public class BankAccountCreatedEvent : BaseEvent
{
    private readonly BankAccount _bankAccount;

    public BankAccountCreatedEvent(BankAccount bankAccount) : base(true)
    {
        _bankAccount = bankAccount;
    }

    public override string Description() => $"Bank account with name {_bankAccount.Name} was created and has Id {_bankAccount.Id}.";
}