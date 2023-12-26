using Mapingway.Application.Contracts.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Authentication.Results;

namespace Mapingway.Application.Features.Auth.Commands.Refresh;

public record RefreshTokenCommand(
    string ExpiredToken, 
    string RefreshToken) : ICommand<RefreshTokenResult>;