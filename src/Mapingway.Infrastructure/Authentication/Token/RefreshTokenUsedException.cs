namespace Mapingway.Infrastructure.Authentication.Token;

public class RefreshTokenUsedException : Exception
{
    public RefreshTokenUsedException(
        string token,
        long? userId = null,
        string? email = null,
        string? message = null)
        : base(message: message ?? $"Refresh token {token} was already used, try to log-in again.")
    {
        Token = token;
        UserId = userId;
        Email = email;
    }

    public string Token { get; init; }
    public long? UserId { get; init; }
    public string? Email { get; init; }
}
