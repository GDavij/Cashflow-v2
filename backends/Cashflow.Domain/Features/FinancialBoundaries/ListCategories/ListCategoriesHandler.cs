﻿using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialBoundaries;

public class ListCategoriesHandler
{
    private readonly ILogger<ListCategoriesHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public ListCategoriesHandler(ILogger<ListCategoriesHandler> logger,
                                 ICashflowDbContext dbContext,
                                 IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Response(long Id, string Name, int TotalTransactionsRegistered, bool Active);
    
    public async Task<IEnumerable<Response>> HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attemping to list categories for user with id {0}.", _authenticatedUser.Id);

        var result = await _dbContext.Categories.Include(c => c.Transactions)
                                                                 .Where(c => c.OwnerId == _authenticatedUser.Id)
                                                                 .Select(c => new Response(c.Id, c.Name, c.Transactions.Count, c.Active))
                                                                 .ToListAsync(cancellationToken);

        _logger.LogInformation("Listed {0} categories for user with id {1}.", result.Count, _authenticatedUser.Id);

        return result;
    }
}


/*
 * Improvements
 * Add an analytical result to see if usage is going closer to limit scope of investment of category
*/