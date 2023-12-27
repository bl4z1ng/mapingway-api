using Mapingway.Application.Contracts.Messaging.Command;

namespace Mapingway.Application.Features.User.Register;

public sealed record CreateUserCommand(
        string Email,
        string Password,
        string FirstName,
        string? LastName) : ICommand<RegisterResult>;
