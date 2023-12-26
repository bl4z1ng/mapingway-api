using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;

namespace Mapingway.Application.Features.User.Register;

public sealed record CreateUserCommand(
        string Email, 
        string Password, 
        string FirstName, 
        string? LastName) : ICommand<RegisterResult>;