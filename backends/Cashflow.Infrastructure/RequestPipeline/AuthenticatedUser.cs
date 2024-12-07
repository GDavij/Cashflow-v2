using Cashflow.Core;
using Cashflow.Domain.Abstractions.RequestPipeline;

namespace Cashflow.Infrastructure.RequestPipeline;

public class AuthenticatedUser : IAuthenticatedUser
{
    public long Id { get; init; }
    public string Email { get; init; }
    public Roles Role { get; init; }
}