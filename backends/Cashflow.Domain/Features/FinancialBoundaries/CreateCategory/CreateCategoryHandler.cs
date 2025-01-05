using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions.FinancialBoundaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialBoundaries;

public class CreateCategoryHandler
{
    private readonly ILogger<CreateCategoryHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public CreateCategoryHandler(ILogger<CreateCategoryHandler> logger, ICashflowDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Request(string Name, double? MaximumBudgetInvestment, decimal? MaximumMoneyInvestment);
    public record Response(long Id);

    public async Task<Response> HandleAsync(Request request)
    {
        _logger.LogInformation("Attemping to create a category with Name \"{0}\".", request.Name);
        
        bool hasAnyCategoryWithSameName = await _dbContext.Categories.AnyAsync(c => c.Name == request.Name &&
                                                                                             c.OwnerId == _authenticatedUser.Id &&
                                                                                             !c.Deleted);
        if (hasAnyCategoryWithSameName)
        {
            _logger.LogError("An attempt to create a category with existent name \"{0}\" was made.", request.Name);
            throw new AttempToDuplicateCategoryNameException(request.Name);
        }

        var category = new Category(request.Name);

        if (request is { MaximumBudgetInvestment: not null, MaximumMoneyInvestment: not null })
        {
            _logger.LogError("An attempt to add two financial constraints to a category is not allowed.");
            throw new AttempToAddMoreThanOneFinancialConstraintException();
        }
        
        if (request.MaximumBudgetInvestment.HasValue)
        {
            _logger.LogInformation("Setting maximum budget investment for category to {0}.", request.MaximumBudgetInvestment);
            category.UseMaximumBudgetInvestmentOf(request.MaximumBudgetInvestment.Value);
        } 
        else if (request.MaximumMoneyInvestment.HasValue)
        {
            _logger.LogInformation("Setting maximum money investment for category to {0}.", request.MaximumMoneyInvestment);
            category.UseMaximumMoneyInvestmentOf(request.MaximumMoneyInvestment.Value);
        }

        _dbContext.Categories.Add(category);

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Created category with name {0}, having Id {1}.", category.Name, category.Id);

        return new Response(category.Id);
    }
}

