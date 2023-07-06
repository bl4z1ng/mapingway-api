using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using Mapingway.API.Internal;
using Mapingway.Application.Contracts.Token.Request;
using Mapingway.Application.Tokens.Commands.Refresh;
using Mapingway.Application.Tokens.Commands.Revoke;
using Mapingway.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand
        (
            request.ExpiredToken,
            request.RefreshToken
        );

        var result = await Mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }

    [Authorize]
    [HttpPost("[action]")]
    public async Task<IActionResult> Revoke(CancellationToken cancellationToken)
    {
        var email = User.Claims.FirstOrDefault(
            claim => claim.Type == WsDecodedClaimTypes.Keys[JwtRegisteredClaimNames.Email])?.Value;

        var command = new RevokeTokenCommand(email);

        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}