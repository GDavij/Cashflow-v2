using Cashflow.Core.Models;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialDistribution.ListBankAccounts;

public class ListBankAccountsHandler
{
    private readonly ILogger<ListBankAccountsHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public ListBankAccountsHandler(ILogger<ListBankAccountsHandler> logger, ICashflowDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Request(string? Name, short? AccountType, int Page, int PageSize);

    public record Response(long Id, string Name, decimal CurrentValue, AccountTypeDto AccountType, bool Active);

    public async Task<IEnumerable<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to list bank accounts for user with id {0}.", _authenticatedUser.Id);

        var bankAccounts = _dbContext.BankAccounts.Include(b => b.AccountType)
                                                  .Where(b => b.OwnerId == _authenticatedUser.Id && !b.Deleted);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            bankAccounts = bankAccounts.Where(b => b.Name.Contains(request.Name));
        }

        if (request.AccountType is not null)
        {
            bankAccounts = bankAccounts.Where(b => b.AccountTypeId == request.AccountType);
        }

        return await bankAccounts.AsNoTracking()
                                 .Select(b => new Response(b.Id,
                                                           b.Name,
                                                           b.CurrentValue,
                                                           new AccountTypeDto(b.AccountType.Id, b.AccountType.Name),
                                                           b.Active)).ToListAsync(cancellationToken);
    }
}
