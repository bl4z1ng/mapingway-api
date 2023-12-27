namespace Mapingway.Presentation.v1.Auth.Requests;

public class RefreshTokenRequest
{
    public required string ExpiredToken { get; init; }
    public required string RefreshToken { get; init; }
}
