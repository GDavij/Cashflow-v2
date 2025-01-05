using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialBoundaries.GetCategorySpendingSummary;

public class GetCategorySpendingSummaryHandler
{
    private readonly ILogger<GetCategoryHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetCategorySpendingSummaryHandler(ILogger<GetCategoryHandler> logger,
        ICashflowDbContext dbContext,
        IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Request(byte Year);

    public record Response(
        long Month,
        decimal CurrentSpent,
        decimal PercentageSpent,
        decimal RemainingDeposit,
        int DaysElapsed,
        decimal DailySpentAverage,
        long ProjectionToSpent,
        List<MostSpentTransactionDto> Top3MostSpentTransactions,
        List<string> Alerts);

    public async Task<List<Response>> GetCategorySpendingSummary(long Id, Request request)
    {
        
    }
}

public record MostSpentTransactionDto
{
    public string Description { get; init; }
    public 
}