using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cashflow.Domain.Features.FinancialDistribution.GetBankAccount;

public class GetBankAccountHandler
{
    private readonly ILogger<GetBankAccountHandler> _logger;
    private readonly ICashflowDbContext _dbContext;
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetBankAccountHandler(ILogger<GetBankAccountHandler> logger, ICashflowDbContext dbContext, IAuthenticatedUser authenticatedUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public record Response(long Id,
                           AccountTypeDto Type,
                           decimal CurrentValue,
                           string Name,
                           long TotalTransactionsRegistered,
                           List<TransactionDto> LastTransactions);

    public async Task<Response> HandleAsync(long id)
    {
        _logger.LogInformation("Attemping to get Bank account with Id {0} for user with id {1}", id, _authenticatedUser.Id);

        var result = await (from bankAccounts in _dbContext.BankAccounts
                            join transactions in _dbContext.Transactions on bankAccounts.Id equals transactions.BankAccountId
                            join transactionMethods in _dbContext.TransactionMethods on transactions.TransactionMethodId equals transactionMethods.Id
                            join categories in _dbContext.Categories on transactions.CategoryId equals categories.Id
                            where bankAccounts.Id == id && !bankAccounts.Deleted && bankAccounts.Active
                            select new Response(bankAccounts.Id,
                                               new AccountTypeDto(bankAccounts.AccountType.Id, bankAccounts.AccountType.Name),
                                               bankAccounts.CurrentValue,
                                               bankAccounts.Name,
                                               bankAccounts.Transactions.Count,
                                               bankAccounts.Transactions.OrderByDescending(t => t.LastModifiedAt)
                                                                        .Take(10)
                                                                        .Select(t => new TransactionDto(t.Id, t.Description, t.DoneAt, t.TransactionMethod.Name, new CategoryDto(t.Category.Id, t.Category.Name)))
                                               .ToList())).FirstOrDefaultAsync();

        if (result is null)
        {
            _logger.LogError("An attempt to get a non existent bank account was made.");
            throw new EntityNotFoundException<BankAccount>();
        }
        _logger.LogInformation("Got bank account with name {0}, having Id {1}.", result.Name, result.Id);
        return result;
    }
}
