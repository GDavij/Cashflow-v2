using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Events.FinancialBoundaries;

public class RemovedMaximumMoneyInvestmentBoundaryEvent : BaseEvent
{
    private readonly Category _category;
    
    public RemovedMaximumMoneyInvestmentBoundaryEvent(Category category) : base(true)
    {
        _category = category;
    }

    public override string Description() => $"Removed Maximum money investment boundary for category {_category.Id}.";
}