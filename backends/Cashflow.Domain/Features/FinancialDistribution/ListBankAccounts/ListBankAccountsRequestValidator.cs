using FluentValidation;

namespace Cashflow.Domain.Features.FinancialDistribution.ListBankAccounts;

public class ListBankAccountsRequestValidator : AbstractValidator<ListBankAccountsHandler.Request>
{
    public ListBankAccountsRequestValidator()
    {
        When(r => !string.IsNullOrEmpty(r.Name), () =>
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name must not be empty.")
                .MaximumLength(60).WithMessage("Name must not be larger than 60 characters.");
        });

        When(r => r.AccountType is not null, () =>
        {
            RuleFor(r => r.AccountType)
                .NotEmpty().WithErrorCode("AccountType must not be empty.");
        });

        RuleFor(r => r.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(r => r.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
    }
}
