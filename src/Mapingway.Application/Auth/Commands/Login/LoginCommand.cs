using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Auth;

namespace Mapingway.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthenticationResult>;