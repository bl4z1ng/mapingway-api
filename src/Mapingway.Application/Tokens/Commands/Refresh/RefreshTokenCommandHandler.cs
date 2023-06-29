using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Abstractions.Messaging.Command;
using Mapingway.Application.Contracts.Token.Result;
using Mapingway.Common.Result;

namespace Mapingway.Application.Tokens.Commands.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IAuthenticationService _authenticationService;


    public RefreshTokenCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }


    public Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var principal = _authenticationService.GetPrincipalFromExpiredToken(command.ExpiredToken);
        
        
    }
}