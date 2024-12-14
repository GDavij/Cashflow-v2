using Cashflow.Core.Events;
using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Events.FinancialDistribution;

public class BankAccountRenamedEvent : BaseEvent
{
    private readonly BankAccount _bankAccount;
    private readonly string _oldName;

    public BankAccountRenamedEvent(BankAccount bankAccount, string oldName)
        : base(true)
    {
        _bankAccount = bankAccount;
        _oldName = oldName;
    }

    public override string Description() => $"Renamed Bank account with id {_bankAccount.Id} from {_oldName} to {_bankAccount.Name}.";
}
