using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Auth.Result;

namespace Mapingway.Application.Auth.Commands.Refresh;

public record RefreshTokenCommand(
    string ExpiredToken, 
    string RefreshToken) : ICommand<RefreshTokenResult>;