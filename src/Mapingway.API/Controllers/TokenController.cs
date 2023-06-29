using System.Net.Mime;
using Mapingway.API.Internal;
using Mapingway.Application.Contracts.Token.Request;
using Mapingway.Application.Tokens.Commands.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.API.Controllers;

[ApiController]
[ApiRoute("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TokenController: BaseApiController
{
    public TokenController(ILoggerFactory loggerFactory, IMediator mediator) 
        : base(loggerFactory, mediator, typeof(TokenController).ToString())
    {
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand
        (
            request.ExpiredToken,
            request.RefreshToken
        );

        var result = await Mediator.Send(command, cancellationToken);
    }
}