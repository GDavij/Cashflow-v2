﻿using Cashflow.Core;

namespace Cashflow.Domain.Entities;

public class User : OwnableEntity<User>
{
    public string Email { get; init; }
    public string Passphrase { get; init; }
    public string Username { get; init; }
    public DateTime BirthDate { get; init; }
    public short RoleId { get; init; }
    public Role Role { get; init; }
}