using Mapingway.Domain.Auth;

namespace Mapingway.Domain;

public class User
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PasswordSalt { get; set; }
    public string? PasswordHash { get; set; }
    public ICollection<Role> Roles { get; set; } = null!;
    public RefreshToken? RefreshToken { get; set; }
    public ICollection<RefreshToken> UsedRefreshTokens { get; set; } = new List<RefreshToken>();
    public string? Created { get; set; }
    public string? Updated { get; set; }
}