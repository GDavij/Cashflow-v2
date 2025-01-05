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
    
    public decimal GetTotalDepositInMonth(short month)
    {
        decimal depositTotal = 0.0M;
        decimal fullTotal = 0.0M;
        foreach (var transaction in Transactions.Where(t => t.Month == month))
        {
            if (transaction.TransactionMethodId == (short)TransactionMethodType.Withdrawl)
            {
                fullTotal -= transaction.Value;
            }
            else if (transaction.TransactionMethodId == (short)TransactionMethodType.Alter && transaction.Value < fullTotal)
            {
                fullTotal -= transaction.Value;
            }
            else if (transaction.TransactionMethodId == (short)TransactionMethodType.Alter && transaction.Value > fullTotal)
            {
                depositTotal += (transaction.Value - depositTotal);
                fullTotal += transaction.Value;
            }
            else
            {
                depositTotal += transaction.Value;
                fullTotal -= transaction.Value;
            }
        }

        return depositTotal;
    }

    public decimal GetTotalWithdrawlInMonth(short month)
    {
        decimal withdrawlTotal = 0.0M;
        decimal fullTotal = 0.0M;
        foreach (var transaction in Transactions.Where(t => t.Month == month))
        {
            if (transaction.TransactionMethodId == (short)TransactionMethodType.Withdrawl)
            {
                withdrawlTotal += transaction.Value;
                fullTotal -= transaction.Value;
            }
            else if (transaction.TransactionMethodId == (short)TransactionMethodType.Alter && transaction.Value < fullTotal)
            {
                withdrawlTotal += (withdrawlTotal - transaction.Value);
                fullTotal -= transaction.Value;
            }
            else if (transaction.TransactionMethodId == (short)TransactionMethodType.Alter && transaction.Value > fullTotal)
            {
                fullTotal += transaction.Value;
            }
            else
            {
                fullTotal -= transaction.Value;
            }
        }

        return withdrawlTotal;
    }

    public decimal GetCurrentValue()
    {
        decimal currentValue = 0.0M;
        Span<Transaction> transactions = Transactions.ToArray();

        for (int i = 0; i < transactions.Length; i++)
        {
            var transaction = transactions[i];
            TransactionMethodType methodType = (TransactionMethodType)transaction.TransactionMethodId;
            switch (methodType)
            {
                case TransactionMethodType.Deposit:
                    currentValue += transaction.Value;
                    break;

                case TransactionMethodType.Withdrawl:
                    currentValue -= transaction.Value;
                    break;

                case TransactionMethodType.Alter:
                    if (transaction.Value < currentValue)
                    {
                        currentValue += (currentValue - transaction.Value);
                    }
                    else
                    {
                        currentValue += (transaction.Value - currentValue);
                    }
                    break;
            }
        }

        return currentValue;
    }

    public bool HasExceedAnyBoundaryForMonth(short month)
    {
        decimal totalWithdrawlForMonth = GetTotalWithdrawlInMonth(month);

        if (MaximumMoneyInvestment.HasValue && totalWithdrawlForMonth > MaximumMoneyInvestment.Value)
        {
            return true;
        }
        else if (MaximumBudgetInvestment.HasValue)
        {
            var totalDepositInMonth = GetTotalDepositInMonth(month);
            if (totalDepositInMonth == 0)
            {
                // If not deposit nothing in month any value will cross the boundary.
                return totalWithdrawlForMonth > 0;
            }
            return (double)Decimal.Divide(totalWithdrawlForMonth, totalDepositInMonth) > MaximumBudgetInvestment.Value;
        }

        return false;
    }
}
