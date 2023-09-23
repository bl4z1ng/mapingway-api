using Mapingway.Application.Abstractions.Messaging.Command;

namespace Mapingway.Application.Tokens.Commands.Logout;

public record LogoutTokenCommand(string Email) : ICommand;