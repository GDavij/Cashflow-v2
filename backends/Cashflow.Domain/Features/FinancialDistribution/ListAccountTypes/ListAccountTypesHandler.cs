using Cashflow.Domain.Abstractions.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialDistribution.ListAccountTypes;

public class ListAccountTypesHandler
{
    public record Response(short Id, string Name);

    private readonly ILogger<ListAccountTypesHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    
    public ListAccountTypesHandler(ILogger<ListAccountTypesHandler> logger, ICashflowDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<Response>> HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to get all account types.");
        
        var accountTypes = await _dbContext.AccountTypes.Select(a => new Response(a.Id, a.Name))
                                                                     .ToListAsync(cancellationToken);
        
        _logger.LogInformation("got all {0} account types.", accountTypes.Count);

        return accountTypes;
    }

}