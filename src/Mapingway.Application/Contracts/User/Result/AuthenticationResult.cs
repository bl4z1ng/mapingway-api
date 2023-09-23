namespace Mapingway.Application.Contracts.User.Result;

public class AuthenticationResult
{
    public string Token { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
    public string UserContextToken { get; init; } = null!;
}