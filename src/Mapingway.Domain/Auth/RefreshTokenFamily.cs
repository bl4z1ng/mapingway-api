namespace Mapingway.Domain.Auth;

public class RefreshTokenFamily
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public User User { get; init; } = null!;
    public ICollection<RefreshToken> Tokens { get; set; } = new HashSet<RefreshToken>();
}