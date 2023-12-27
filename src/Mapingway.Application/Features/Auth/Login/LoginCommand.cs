using Mapingway.Application.Contracts.Messaging.Command;

namespace Mapingway.Application.Features.Auth.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
