using Mapingway.Application.Contracts.Abstractions.Messaging.Command;

namespace Mapingway.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(string Email, string RefreshToken) : ICommand;