using Mapingway.Domain.Auth;

namespace Mapingway.Domain.User;

public class User
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PasswordSalt { get; set; }
    public string? PasswordHash { get; set; }
    public ICollection<Role> Roles { get; set; } = null!;
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public string? Created { get; set; }
    public string? Updated { get; set; }
}