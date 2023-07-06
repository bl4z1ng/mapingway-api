using Mapingway.Application.Abstractions.Messaging.Command;

namespace Mapingway.Application.Tokens.Commands.Revoke;

public record RevokeTokenCommand(string Email) : ICommand;