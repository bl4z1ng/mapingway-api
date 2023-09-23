namespace Mapingway.API.Internal.Contracts;

public class RefreshResponse
{
    public string Token { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}