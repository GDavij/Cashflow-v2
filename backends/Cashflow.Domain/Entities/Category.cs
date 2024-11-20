using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class Category : OwnableEntity<long, long>
{
    public float? MaximumBudgetInvestment { get; private set; }
    public decimal? MaximumMoneyInvestment { get; private set; }
    public string Name { get; private set; }
    
    public ICollection<Transaction> Transactions { get; init; }
}