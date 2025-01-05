using System.Net;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Cashflow.Domain.Exceptions.FinancialBoundaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialBoundaries;

public class UpdateCategoryHandler
{
    private readonly ILogger<UpdateCategoryHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public UpdateCategoryHandler(ILogger<UpdateCategoryHandler> logger,
                                 ICashflowDbContext dbContext,
                                 IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Request(string Name, double? MaximumBudgetInvestment, decimal? MaximumMoneyInvestment, bool Active);
    public record Response(long Id);

    public async Task<Response> HandleAsync(long id, Request request)
    {
        _logger.LogInformation("Attemping to update category with id {0}.", id);

        bool hasAnyCategoryWithSameName = await _dbContext.Categories.AnyAsync(c => c.Name == request.Name &&
                                                                                             c.OwnerId == _authenticatedUser.Id &&
                                                                                             c.Id != id &&
                                                                                             !c.Deleted);
        if (hasAnyCategoryWithSameName)
        {
            _logger.LogError("An attempt to create a category with existent name \"{0}\" was made.", request.Name);
            throw new AttempToDuplicateCategoryNameException(request.Name);
        }

        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id &&
                                                                            c.OwnerId == _authenticatedUser.Id &&
                                                                            !c.Deleted);
        if (category is null)
        {
            _logger.LogError("Could not found category with id {0} for user with id {1}.", id, _authenticatedUser.Id);
            throw new EntityNotFoundException<Category>();
        }

        category.ChangeNameTo(request.Name);

        if (request is { MaximumBudgetInvestment: not null, MaximumMoneyInvestment: not null })
        {
            _logger.LogError("An attempt to add two financial constraints to a category is not allowed.");
            throw new AttempToAddMoreThanOneFinancialConstraintException();
        }

        if (request.MaximumBudgetInvestment.HasValue)
        {
            _logger.LogInformation("Setting maximum budget investment for category with id {0}.", category.Id);
            category.RemoveMaximumMoneyInvestmentBoundary();
            category.UseMaximumBudgetInvestmentOf(request.MaximumBudgetInvestment.Value);
        }
        else if (request.MaximumMoneyInvestment.HasValue)
        {
            _logger.LogInformation("Setting maximum money investment for category with id {0}.", category.Id);
            category.RemoveMaximumBudgetInvestmentBoundary();
            category.UseMaximumMoneyInvestmentOf(request.MaximumMoneyInvestment.Value);
        }
        else
        {
            _logger.LogInformation("Removing any financial constraint for category with id {0}.", category.Id);
            category.RemoveMaximumMoneyInvestmentBoundary();
            category.RemoveMaximumBudgetInvestmentBoundary();
        }

        if (request.Active)
        {
            _logger.LogInformation("Activating category with id {0}.", category.Id);
            category.Activate();
        }
        else
        {
            _logger.LogInformation("Deactivating category with id {0}.", category.Id);
            category.Deactivate();
        }

        await _dbContext.SaveChangesAsync();
        return new Response(category.Id);
    }
}

