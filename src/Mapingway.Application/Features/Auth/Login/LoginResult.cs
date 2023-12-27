namespace Mapingway.Application.Features.Auth.Login;

public class LoginResult
{
    public required string Token { get; init; } = null!;
    public required string RefreshToken { get; init; } = null!;
    public required string UserContextToken { get; init; } = null!;
}
