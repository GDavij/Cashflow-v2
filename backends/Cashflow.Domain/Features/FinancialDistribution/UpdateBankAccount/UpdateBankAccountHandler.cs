using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialDistribution.UpdateBankAccount;

public class UpdateBankAccountHandler
{
    private readonly ILogger<UpdateBankAccountHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;
    
    public UpdateBankAccountHandler(ILogger<UpdateBankAccountHandler> logger, ICashflowDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }
    
    public record Request(string Name);
    
    public record Response(long Id);
    
    public async Task<Response> HandleAsync(long id, Request request)
    {
        _logger.LogInformation("Attemping to update Bank account with Id {0} name to {1} for user with id {2}", id, request.Name, _authenticatedUser.Id);
        BankAccount? bankAccount = await _dbContext.BankAccounts.FirstOrDefaultAsync(b => b.Id == id && b.OwnerId == _authenticatedUser.Id);
        if (bankAccount is null)
        {
            _logger.LogError("An attempt to update a non existent bank account was made.");
            throw new EntityNotFoundException<BankAccount>();
        }

        bankAccount.RenameTo(request.Name);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Updated bank account with name {0}, having Id {1}.", bankAccount.Name, bankAccount.Id);
        return new Response(bankAccount.Id);
    }
}