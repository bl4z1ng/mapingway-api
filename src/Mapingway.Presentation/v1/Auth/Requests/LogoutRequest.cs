namespace Mapingway.Presentation.v1.Auth.Requests;

public class LogoutRequest
{
    public required string RefreshToken { get; init; }
}
