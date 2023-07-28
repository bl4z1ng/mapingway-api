namespace Mapingway.Domain.Auth;

public class RefreshTokenFamily
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public User User { get; init; } = null!;
    public ICollection<RefreshToken> Tokens { get; set; } = new List<RefreshToken>();
}