using Cashflow.Core;
using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Events.FinancialBoundaries;

public class CategoryCreatedEvent : BaseEvent
{
    private readonly Category Category;
    public CategoryCreatedEvent(Category category) : base(true)
    {
        Category = category;
    }

    public override string Description() => $"Category was created and has Id {Category.Id}";
}
