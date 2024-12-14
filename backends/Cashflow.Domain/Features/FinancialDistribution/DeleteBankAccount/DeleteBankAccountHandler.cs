using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialDistribution.DeleteBankAccount;

public class DeleteBankAccountHandler
{
    private readonly ILogger<DeleteBankAccountHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public DeleteBankAccountHandler(ILogger<DeleteBankAccountHandler> logger, ICashflowDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Response(long Id);

    public async Task<Response> HandleAsync(long id)
    {
        _logger.LogInformation("Attemping to delete Bank account with Id {0} for user with id {1}", id, _authenticatedUser.Id);
        BankAccount? bankAccount = await _dbContext.BankAccounts.FirstOrDefaultAsync(b => b.Id == id && b.OwnerId == _authenticatedUser.Id);
        if (bankAccount is null)
        {
            _logger.LogError("An attempt to delete a non existent bank account was made.");
            throw new EntityNotFoundException<BankAccount>();
        }
        bankAccount.Delete();
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Deleted bank account with name {0}, having Id {1}.", bankAccount.Name, bankAccount.Id);

        return new Response(bankAccount.Id);
    }
}
