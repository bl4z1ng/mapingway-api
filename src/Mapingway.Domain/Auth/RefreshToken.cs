namespace Mapingway.Domain.Auth;

public class RefreshToken
{
    public int Id { get; init; }
    public string Value { get; init; } = null!;
    public int UserId { get; init; }
    public User User { get; init; } = null!;
    public bool IsUsed { get; set; }
    public DateTime ExpiresAt { get; init; }
}