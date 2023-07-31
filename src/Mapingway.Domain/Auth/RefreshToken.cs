namespace Mapingway.Domain.Auth;

public class RefreshToken
{
    public long Id { get; init; }
    public string Value { get; init; } = null!;
    public long? UserId { get; init; }
    public User? User { get; set; }
    public long? TokenFamilyId { get; init; }
    public RefreshTokenFamily? TokenFamily { get; init; }
    public bool IsUsed { get; set; }
    public DateTime ExpiresAt { get; init; }
}