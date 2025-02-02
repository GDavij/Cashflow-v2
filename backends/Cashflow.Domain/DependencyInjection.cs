using Cashflow.Domain.Features.FinancialBoundaries;
using Cashflow.Domain.Features.FinancialDistribution.CreateBankAccount;
using Cashflow.Domain.Features.FinancialDistribution.DeleteBankAccount;
using Cashflow.Domain.Features.FinancialDistribution.GetBankAccount;
using Cashflow.Domain.Features.FinancialDistribution.ListAccountTypes;
using Cashflow.Domain.Features.FinancialDistribution.ListBankAccounts;
using Cashflow.Domain.Features.FinancialDistribution.UpdateBankAccount;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cashflow.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<DeleteCategoryHandler>();
        services.AddScoped<GetCategoryHandler>();
        services.AddScoped<ListCategoriesHandler>();
        services.AddScoped<UpdateCategoryHandler>();

        services.AddScoped<CreateBankAccountHandler>();
        services.AddScoped<UpdateBankAccountHandler>();
        services.AddScoped<DeleteBankAccountHandler>();
        services.AddScoped<ListBankAccountsHandler>();
        services.AddScoped<GetBankAccountHandler>();

        services.AddScoped<ListAccountTypesHandler>();

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        //Is possible to decouple this in future using Assembly markers.
        services.AddScoped<CreateCategoryRequestValidator>();
        services.AddScoped<UpdateCategoryRequestValidator>();

        services.AddScoped<CreateBankAccountRequestValidator>();
        services.AddScoped<UpdateBankAccountRequestValidator>();
        services.AddScoped<ListBankAccountsRequestValidator>();
        
        return services;
    }
}