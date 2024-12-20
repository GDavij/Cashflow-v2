using Cashflow.Core;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.EventHandling;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Infrastructure.DataAccess.Contexts;
using Cashflow.Infrastructure.EventHandling;
using Cashflow.Infrastructure.RequestPipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cashflow.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddEventReaction(this IServiceCollection services)
    {
        services.AddScoped<IEventMediator, MediatrEventMediator>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Domain.DependencyInjection).Assembly);
        });

        return services;
    }

    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<ICashflowDbContext, CashflowDbContext>(cfg =>
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                cfg.EnableSensitiveDataLogging();
            }
            
            cfg.UseAzureSql(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    public static IServiceCollection AddRequestPipeline(this IServiceCollection services)
    {
        var mockedAuthenticatedUser = new AuthenticatedUser
        {
            Email = "mocked@email.com",
            Id = 1,
            Role = Roles.Sudo
        };

        services.AddScoped<IAuthenticatedUser, AuthenticatedUser>(_ => mockedAuthenticatedUser);

        return services;
    }
}