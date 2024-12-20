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
                           List<TransactionDto> LastTransactions,
                           bool Active);

    public async Task<Response> HandleAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attemping to get Bank account with Id {0} for user with id {1}", id, _authenticatedUser.Id);

        var result = await (from bankAccounts in _dbContext.BankAccounts.Where(b => b.OwnerId == _authenticatedUser.Id)
                            join transactions in _dbContext.Transactions.Where(t => !t.Deleted) on bankAccounts.Id equals transactions.BankAccountId into transactionsHistory
                            from transactions in transactionsHistory.DefaultIfEmpty()
                            join transactionMethods in _dbContext.TransactionMethods on transactions.TransactionMethodId equals transactionMethods.Id into transactionMethodsGroup
                            from transactionMethods in transactionMethodsGroup.DefaultIfEmpty()
                            join categories in _dbContext.Categories on transactions.CategoryId equals categories.Id into categoriesGroup
                            from categories in categoriesGroup.DefaultIfEmpty()
                            where bankAccounts.Id == id && !bankAccounts.Deleted
                            select new Response(bankAccounts.Id,
                                               new AccountTypeDto(bankAccounts.AccountType.Id, bankAccounts.AccountType.Name),
                                               bankAccounts.CurrentValue,
                                               bankAccounts.Name,
                                               bankAccounts.Transactions.Count,
                                               bankAccounts.Transactions.OrderByDescending(t => t.LastModifiedAt)
                                                                        .Take(10)
                                                                        .Select(t => new TransactionDto(t.Id, t.Description, t.DoneAt, t.TransactionMethod.Name, t.Category != null 
                                                                                                                                                                    ? new CategoryDto(t.Category.Id, t.Category.Name)
                                                                                                                                                                    : null)).ToList(),
                                               bankAccounts.Active)).FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            _logger.LogError("An attempt to get a non existent bank account was made.");
            throw new EntityNotFoundException<BankAccount>();
        }
        _logger.LogInformation("Got bank account with name {0}, having Id {1}.", result.Name, result.Id);
        return result;
    }
}
