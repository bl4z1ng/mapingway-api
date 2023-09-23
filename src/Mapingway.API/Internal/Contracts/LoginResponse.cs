namespace Mapingway.API.Internal.Contracts;

public class LoginResponse
{
    public string Token { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}