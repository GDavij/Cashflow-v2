﻿using FluentValidation;

namespace Cashflow.Domain.Features.FinancialBoundaries;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryHandler.Request>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name must not be empty.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
            .MaximumLength(60).WithMessage("Name must not exceed 60 characters.");

        RuleFor(r => r.MaximumMoneyInvestment)
            .LessThanOrEqualTo(0).When(r => r.MaximumMoneyInvestment is not null).WithMessage("Maximum money investment must be viable.");
        
        RuleFor(r => r.MaximumBudgetInvestment)
            .LessThanOrEqualTo(0).When(r => r.MaximumBudgetInvestment is not null).WithMessage("Maximum budget investment must be viable.");
    }
}