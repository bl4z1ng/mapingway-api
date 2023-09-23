using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Auth.Result;

namespace Mapingway.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthenticationResult>;