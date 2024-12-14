using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions.FinancialDistribution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialDistribution.CreateBankAccount;

public class CreateBankAccountHandler
{
    private readonly ILogger<CreateBankAccountHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;
    
    public CreateBankAccountHandler(ILogger<CreateBankAccountHandler> logger, ICashflowDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Request(short AccountType, string Name, decimal? InitialValue);

    public record Response(long Id);

    public async Task<Response> HandleAsync(Request request)
    {
        _logger.LogInformation("Attemping to create a Bank account with type {0} and name {1} for user with id {2}", request.AccountType, request.Name, _authenticatedUser.Id);
        bool accountTypeExists = await _dbContext.AccountTypes.AnyAsync(a => a.Id == request.AccountType);
        if (accountTypeExists is false)
        {
            _logger.LogError("An attempt to create a bank account with non existent account type {0} was made.", request.AccountType);
            throw new AttemptToCreateBankAccountWithNonExistentAccountTypeException(request.AccountType);
        }

        BankAccount bankAccount;
        if (request.InitialValue is not null)
        {
            bankAccount = new BankAccount(request.AccountType, request.Name, request.InitialValue.Value);
        }
        else
        {
            bankAccount = new BankAccount(request.AccountType, request.Name);
        }

        _dbContext.BankAccounts.Add(bankAccount);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Created bank account with name {0}, having Id {1}.", bankAccount.Name, bankAccount.Id);
        return new Response(bankAccount.Id);
    }
}
