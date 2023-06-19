namespace Mapingway.Application.Contracts.User;

public class RegisterRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string? Role { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}