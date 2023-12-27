namespace Mapingway.Application.Features.Auth.Login;

public class LoginResult
{
    public string Token { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
    public string UserContextToken { get; init; } = null!;
}
