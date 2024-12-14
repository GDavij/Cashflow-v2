using Cashflow.Core;

namespace Cashflow.Core.Events.FinancialDistribution;

public class BankAccountCreatedEvent<T> : BaseEvent
    where T : OwnableEntity<T>
{
    private readonly T _bankAccount;

    public BankAccountCreatedEvent(T bankAccount) : base(true)
    {
        _bankAccount = bankAccount;
    }

    public override string Description() => $"Bank account with name {_bankAccount.Name} was created and has Id {_bankAccount.Id}.";
}