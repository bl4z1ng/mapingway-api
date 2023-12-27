namespace Mapingway.Presentation.v1.Auth.Requests;

public sealed class LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
