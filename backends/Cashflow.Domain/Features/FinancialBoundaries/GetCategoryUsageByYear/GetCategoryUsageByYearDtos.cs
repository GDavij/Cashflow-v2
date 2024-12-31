using Cashflow.Domain.Enums;

namespace Cashflow.Domain.Features.FinancialBoundaries.GetCategoryUsageByYear;

public record TransactionsUsageAggregate(short Month, long TotalTransactions, decimal TotalDeposit, decimal TotalWithdrawl, bool HasReachedLimit);
