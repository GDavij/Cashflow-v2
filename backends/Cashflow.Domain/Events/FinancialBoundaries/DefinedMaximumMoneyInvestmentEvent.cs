using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Events.FinancialBoundaries;

public class DefinedMaximumMoneyInvestmentEvent : BaseEvent
{
    private readonly Category _category;

    public DefinedMaximumMoneyInvestmentEvent(Category category)
    {
        _category = category;
    }
    
    public override string Description() => $"Defined maximum money investment for category {_category.Id} to ${_category.MaximumMoneyInvestment}";
}