namespace Mapingway.Presentation.v1.Auth.Responses;

public class LoginResponse
{
    public string Token { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}
