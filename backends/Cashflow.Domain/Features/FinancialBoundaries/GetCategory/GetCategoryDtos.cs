using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Features.FinancialBoundaries.GetCategory;

public record TransactionDto(long Id, string Description, DateTime DoneAt, string TransactionMethod, BankAccountDto? BankAccount);
public record CategoryDto(long Id, string Name);
public record BankAccountDto(long Id, string Name);

