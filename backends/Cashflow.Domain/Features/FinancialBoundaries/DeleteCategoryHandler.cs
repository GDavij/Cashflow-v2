using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialBoundaries;

public class DeleteCategoryHandler
{
    private readonly ILogger<DeleteCategoryHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public DeleteCategoryHandler(ILogger<DeleteCategoryHandler> logger, ICashflowDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Response(long CategoryId);

    public async Task<Response> HandleAsync(long id)
    {
        _logger.LogInformation("Attemping to delete category with id {0}.", id);

        var existentCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id &&
                                                                                            c.OwnerId == _authenticatedUser.Id &&
                                                                                            !c.Deleted);
        if (existentCategory is null)
        {
            throw new EntityNotFoundException<Category>();
        }
        
        existentCategory.Delete();
        await _dbContext.SaveChangesAsync();
        
        return new Response(existentCategory.Id);
    }
}