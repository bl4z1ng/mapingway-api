﻿using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using Mapingway.API.Internal;
using Mapingway.API.Swagger.Examples.Responses.Token;
using Mapingway.Application.Contracts.Token.Request;
using Mapingway.Application.Contracts.Token.Result;
using Mapingway.Application.Tokens.Commands.Refresh;
using Mapingway.Application.Tokens.Commands.Revoke;
using Mapingway.Common.Constants;
using Mapingway.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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

    /// <summary>
    /// Refreshes access and refresh tokens and invalidates passed refresh token.
    /// </summary>
    /// <returns>
    /// A newly generated Bearer access and refresh tokens.
    /// </returns>
    /// <response code="200">Returns the newly created access and refresh tokens</response>
    /// <response code="400">If the refresh token is invalid or already used</response>
    [ProducesResponseType(typeof(RefreshTokenResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RefreshToken400ErrorResultExample))]
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
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Invalidates refresh token for current user.
    /// </summary>
    /// <response code="200">Token is successfully invalidated</response>
    /// <response code="400">If the user has no valid refresh token</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RevokeToken400ErrorResultExample))]
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