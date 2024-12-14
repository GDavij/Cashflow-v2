using Cashflow.Core.Events;
using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Events.FinancialBoundaries;

public class RemovedMaximumBudgetInvestmentPercentBoundaryEvent : BaseEvent
{
    private readonly Category _category;

    public RemovedMaximumBudgetInvestmentPercentBoundaryEvent(Category category) : base(true)
    {
        _category = category;
    }
    
    public override string Description() => $"Removed maximum budget investment percent boundary for category {_category.Id}.";
}