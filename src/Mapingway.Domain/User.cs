using Mapingway.Domain.Auth;

namespace Mapingway.Domain;

public class User
{
    public long Id { get; set; }
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string? LastName { get; set; }
    public string? PasswordSalt { get; set; }
    public string? PasswordHash { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = null!;
    public ICollection<Role> Roles { get; set; } = null!;
    public RefreshTokenFamily RefreshTokensFamily { get; set; } = null!;
    public string? Created { get; set; }
    public string? Updated { get; set; }
}
