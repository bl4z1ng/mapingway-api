using Mapingway.Application.Contracts.Messaging.Command;

namespace Mapingway.Application.Features.Auth.Refresh;

public record RefreshTokenCommand(string Email, string RefreshToken) : ICommand<RefreshTokenResult>;
