namespace Mapingway.Application.Contracts.User;

public sealed class LoginRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}