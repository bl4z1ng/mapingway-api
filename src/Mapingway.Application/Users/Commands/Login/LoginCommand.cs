using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.User.Result;

namespace Mapingway.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthenticationResult>;