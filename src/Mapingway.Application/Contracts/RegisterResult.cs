namespace Mapingway.Application.Contracts;

public class RegisterResult
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}