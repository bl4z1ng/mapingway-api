using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Authentication.Results;

namespace Mapingway.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthenticationResult>;