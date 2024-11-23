using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Events.FinancialBoundaries;

public class DefinedMaximumBudgetInvestmentPercentEvent : BaseEvent
{
    private readonly Category _category;

    public DefinedMaximumBudgetInvestmentPercentEvent(Category category)
    {
        _category = category;
    }
    
    public override string Description() => $"Defined maximum budget investment percent for category {_category.Id} to {_category.MaximumBudgetInvestment}%";
}