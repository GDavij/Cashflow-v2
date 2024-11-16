using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class User : OwnableEntity<long, long>
{
    public string Email { get; init; }
    public string Passphrase { get; init; }
    public string UserName { get; init; }
    public DateTime BirthDate { get; init; }
    public long RoleId { get; init; }
    public Role Role { get; init; }
    
}