using FluentValidation;

namespace Cashflow.Domain.Features.FinancialDistribution.CreateBankAccount;

public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountHandler.Request>
{
    public CreateBankAccountRequestValidator()
    {
        RuleFor(c => c.AccountType)
            .NotEmpty().WithMessage("Account type must not be empty.");
        
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty.")
            .MaximumLength(60).WithMessage("Name must not be larger than 60 characters.");

        When(c => c.InitialValue is not null, () =>
        {
            RuleFor(c => c.InitialValue)
                .NotEmpty().WithMessage("Initial value must not be empty.");
        });
    }
}