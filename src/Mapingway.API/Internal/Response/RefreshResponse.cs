namespace Mapingway.API.Internal.Response;

public class RefreshResponse
{
    public string Token { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}