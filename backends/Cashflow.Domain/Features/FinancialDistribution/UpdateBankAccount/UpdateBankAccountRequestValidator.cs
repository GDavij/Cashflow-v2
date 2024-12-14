using FluentValidation;

namespace Cashflow.Domain.Features.FinancialDistribution.UpdateBankAccount;

public class UpdateBankAccountRequestValidator : AbstractValidator<UpdateBankAccountHandler.Request>
{
    public UpdateBankAccountRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty.")
            .MaximumLength(60).WithMessage("Name must not be larger than 60 characters.");
    }
}
