using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Authentication;

namespace Mapingway.Application.Features.Auth.Refresh;

public record RefreshTokenCommand(
    string ExpiredToken, 
    string RefreshToken) : ICommand<RefreshTokenResult>;