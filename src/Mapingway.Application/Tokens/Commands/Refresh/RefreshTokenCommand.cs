using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Token.Result;

namespace Mapingway.Application.Tokens.Commands.Refresh;

public record RefreshTokenCommand(
    string ExpiredToken, 
    string RefreshToken) : ICommand<RefreshTokenResult>;