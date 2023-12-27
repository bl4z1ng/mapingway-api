using Mapingway.Application.Contracts.Messaging.Command;

namespace Mapingway.Application.Features.Auth.Logout;

public record LogoutCommand(string Email, string RefreshToken) : ICommand;
