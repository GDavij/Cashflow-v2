using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialBoundaries;

public class GetCategoryHandler
{
    private readonly ILogger<GetCategoryHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetCategoryHandler(ILogger<GetCategoryHandler> logger,
                              ICashflowDbContext dbContext,
                              IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Response(long Id, string Name, double? MaximumBudgetInvestment, decimal? MaximumMoneyInvestment, long TotalTransactionsRegistered, bool Active);

    public async Task<Response> HandleAsync(long id, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Categories.Include(c => c.Transactions)
                                                .ThenInclude(t => t.BankAccount)
                                                .Include(c => c.Transactions)
                                                .ThenInclude(c => c.TransactionMethod)
                                                .AsNoTracking()
                                                .Where(c => c.Id == id && !c.Deleted && c.OwnerId == _authenticatedUser.Id)
                                                .Select(c => new Response(c.Id,
                                                                          c.Name,
                                                                          c.MaximumBudgetInvestment,
                                                                          c.MaximumMoneyInvestment,
                                                                          c.Transactions.Count,
                                                                          c.Active))
                                                .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            throw new EntityNotFoundException<Category>();
        }

        return result;
    }

}

/*
 * Improvements
 * Add an analytical result to see if usage is going closer to limit scope of investment of category
 */