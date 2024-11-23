using Cashflow.Core;
using Cashflow.Domain.Events;
using Cashflow.Domain.Events.FinancialBoundaries;

namespace Cashflow.Domain.Entities;

public class Category : OwnableEntity<long, long>
{
    public float? MaximumBudgetInvestment { get; private set; }
    public decimal? MaximumMoneyInvestment { get; private set; }
    public string Name { get; private set; }
    
    public ICollection<Transaction> Transactions { get; init; }

    public Category()
    { }
    
    public Category(string name)
    {
        Name = name;
        RaiseEvent(new CategoryCreatedEvent(this));
    }

    public void ChangeNameTo(string newName)
    {
        var oldName = Name;
        Name = newName;
        RaiseEvent(new ChangeCategoryNameEvent(this, oldName));
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

    public void UseMaximumBudgetInvestmentOf(float maxBudgetInvestmentPercent)
    {
        if (!MaximumBudgetInvestment.HasValue)
        {
            return;
        }
        
        MaximumBudgetInvestment = maxBudgetInvestmentPercent;
        RaiseEvent(new DefinedMaximumBudgetInvestmentPercentEvent(this));
    }

    public void RemoveMaximumBudgetInvestmentBoundary()
    {
        MaximumBudgetInvestment = null;
        RaiseEvent(new RemovedMaximumBudgetInvestmentPercentBoundaryEvent(this));
    }
}

