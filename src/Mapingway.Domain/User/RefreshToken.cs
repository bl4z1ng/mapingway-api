namespace Mapingway.Domain.User;

public class RefreshToken
{
    public int Id { get; init; }
    public string Value { get; init; } = null!;
    public User User { get; init; } = null!;
    public bool IsUsed { get; init; }
    public DateTime ExpiresAt { get; init; }
}