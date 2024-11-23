using Cashflow.Core;

namespace Cashflow.Domain.Abstractions.RequestPipeline;

public interface IAuthenticatedUser
{
    public long Id { get; init; }
    public string Email { get; init; }
    public Roles Role { get; init; }
}