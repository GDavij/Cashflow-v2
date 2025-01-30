using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class User : OwnableEntity<User>
{
    public required string Email { get; init; }
    public required string Passphrase { get; init; }
    public required string Username { get; init; }
    public DateTime BirthDate { get; init; }
    public short RoleId { get; init; }
    public Role Role { get; init; }
}