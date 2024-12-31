using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Enums;
using Cashflow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialBoundaries.GetCategoryUsageByYear;

public class GetCategoryUsageByYearHandler
{
    private readonly ILogger<GetCategoryUsageByYearHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetCategoryUsageByYearHandler(ILogger<GetCategoryUsageByYearHandler> logger,
                                         ICashflowDbContext dbContext,
                                         IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Request(short Year, long? MergeWithBankAccountId);

    public record Response(short Year, List<TransactionsUsageAggregate> TransactionsUsageAggregate);

    public async Task<Response> HandleAsync(long categoryId, Request request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting category usage for category with id {0}.", categoryId);
        if (request.MergeWithBankAccountId is not null)
        {
            var hasBankAccount = await _dbContext.BankAccounts.AnyAsync(b => b.Id == request.MergeWithBankAccountId && b.OwnerId == _authenticatedUser.Id);
            if (hasBankAccount is false)
            {
                throw new EntityNotFoundException<BankAccount>();
            }
        }


        var category = await _dbContext.Categories.Include(t => t.Transactions.Where(t => t.Year == request.Year))
                                                  .Where(c => c.Id == categoryId && !c.Deleted && c.OwnerId == _authenticatedUser.Id)
                                                  .FirstOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            throw new EntityNotFoundException<Category>();
        }

        var transactionAggregates = new List<TransactionsUsageAggregate>(12);
        for (short month = 1; month <= 12; month++)
        {
            transactionAggregates.Add(new TransactionsUsageAggregate(month, category.GetTotalTransactionsMadeInMonth(month), category.GetTotalDepositInMonth(month), category.GetTotalWithdrawlInMonth(month), category.HasExceedAnyBoundaryForMonth(month)));
        }

        _logger.LogInformation("Category usage for category with id {0} has been retrieved.", categoryId);
        return new Response(request.Year, transactionAggregates);
    }
}
