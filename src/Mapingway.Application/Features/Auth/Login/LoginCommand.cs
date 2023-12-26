using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Abstractions.Messaging.Command;

namespace Mapingway.Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthenticationResult>;