﻿using Cashflow.Domain.Abstractions.DataAccess;
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

    public record Response(long Id, string Name, float? MaximumBudgetInvestment, decimal? MaximumMoneyInvestment, int TotalTransactionsRegistered);

    public async Task<Response> HandleAsync(long id)
    {
        var category = await _dbContext.Categories.Include(c => c.Transactions)
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(c => c.Id == id &&
                                                                                    !c.Deleted &&
                                                                                    c.OwnerId == _authenticatedUser.Id);

        if (category is null)
        {
            throw new EntityNotFoundException<Category>();
        }
        
        return new Response(category.Id, category.Name, category.MaximumBudgetInvestment, category.MaximumMoneyInvestment, category.Transactions.Count);
    }

}