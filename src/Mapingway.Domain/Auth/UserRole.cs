﻿namespace Mapingway.Domain.Auth;

public class UserRole
{
    public long UserId { get; init; }
    public User User { get; init; } = null!;
    public int RoleId { get; init; }
    public Role Role { get; init; } = null!;
}