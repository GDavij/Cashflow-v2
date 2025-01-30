using Cashflow.Core;
using Cashflow.Domain.Enums;
using Cashflow.Domain.Events;
using Cashflow.Domain.Events.FinancialBoundaries;

namespace Cashflow.Domain.Entities;

public class Category : OwnableEntity<Category>
{
    public double? MaximumBudgetInvestment { get; private set; }
    public decimal? MaximumMoneyInvestment { get; private set; }
    public string Name { get; private set; }

    public ICollection<Transaction> Transactions { get; init; } = new List<Transaction>();

    public Category() : base()
    { }

    public Category(string name) : base()
    {
        Name = name;
    }

    public void ChangeNameTo(string newName)
    {
        if (newName == Name)
        {
            return;
        }

        RaiseEvent(new ChangeCategoryNameEvent(this, Name));
        Name = newName;
    }

    public void UseMaximumMoneyInvestmentOf(decimal maxMoneyInvestmentValue)
    {
        MaximumMoneyInvestment = maxMoneyInvestmentValue;
        RaiseEvent(new DefinedMaximumMoneyInvestmentEvent(this));
    }

    public void RemoveMaximumMoneyInvestmentBoundary()
    {
        if (!MaximumMoneyInvestment.HasValue)
        {
            return;
        }

        MaximumMoneyInvestment = null;
        RaiseEvent(new RemovedMaximumMoneyInvestmentBoundaryEvent(this));
    }

    public void UseMaximumBudgetInvestmentOf(double maxBudgetInvestmentPercent)
    {
        MaximumBudgetInvestment = maxBudgetInvestmentPercent;
        RaiseEvent(new DefinedMaximumBudgetInvestmentPercentEvent(this));
    }

    public void RemoveMaximumBudgetInvestmentBoundary()
    {
        if (!MaximumBudgetInvestment.HasValue)
        {
            return;
        }

        MaximumBudgetInvestment = null;
        RaiseEvent(new RemovedMaximumBudgetInvestmentPercentBoundaryEvent(this));
    }
}
