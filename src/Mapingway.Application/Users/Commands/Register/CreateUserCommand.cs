using Mapingway.Application.Abstractions.Messaging.Command;

namespace Mapingway.Application.Users.Commands.Register;

public sealed record CreateUserCommand(
        string Email, 
        string Password, 
        string? FirstName, 
        string? LastName) : ICommand<int>;