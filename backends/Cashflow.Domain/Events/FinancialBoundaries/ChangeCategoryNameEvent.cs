using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Events.FinancialBoundaries;

public class ChangeCategoryNameEvent : BaseEvent
{
    private readonly Category _category;
    private readonly string _oldName;


    public ChangeCategoryNameEvent(Category category, string oldName)
    {
        _category = category;
        _oldName = oldName;
    }

    public override string Description() => $"Updated category with id {_category.Id} name from {_oldName} to {_category.Name}";
}