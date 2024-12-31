using Cashflow.Domain.Entities;

namespace Cashflow.Domain.Features.FinancialBoundaries.GetCategory;

public record CategoryDto(long Id, string Name);
public record BankAccountDto(long Id, string Name);

