﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using MediatR;
using Swashbuckle.AspNetCore.Filters;
using Mapingway.API.Internal;
using Mapingway.API.Internal.Mapping;
using Mapingway.API.Internal.Contracts;
using Mapingway.API.Swagger.Documentation;
using Mapingway.API.Swagger.Examples.Results.Auth;
using Mapingway.Application.Contracts.Auth.Request;
using Mapingway.Common.Constants;
using Mapingway.Common.Result;
using Mapingway.Infrastructure.Authentication;

namespace Mapingway.API.Controllers;

[ApiRoute("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerControllerOrder(1)]
public class AuthController : BaseApiController
{
    private readonly IRequestToCommandMapper _requestToCommandMapper;
    private readonly IResultToResponseMapper _resultToResponseMapper;


    public AuthController(
        ILoggerFactory loggerFactory, 
        IMediator mediator, 
        IRequestToCommandMapper requestToCommandMapper,
        IResultToResponseMapper resultToResponseMapper) : 
        base(loggerFactory, mediator, typeof(AuthController).ToString())
    {
        _requestToCommandMapper = requestToCommandMapper;
        _resultToResponseMapper = resultToResponseMapper;
    }


    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <returns>
    /// A newly generated Bearer token.
    /// </returns>
    /// <response code="200">User is successfully logged in.</response>
    /// <response code="401">If user credentials are not valid.</response>
    /// <response code="409">If the user is already authenticated.</response>
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(Authentication401ErrorResultExample))]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            return Conflict("User is already authenticated");
        }

        var command = _requestToCommandMapper.Map(request);
        var result = await Mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Failure(result, Unauthorized);
        }

        UpdateUserContextToken(result.Value!.UserContextToken);
        var response = _resultToResponseMapper.Map(result.Value);

        return Ok(response);
    }

    /// <summary>
    /// Refreshes access and refresh tokens and invalidates passed refresh token.
    /// </summary>
    /// <returns>
    /// A newly generated Bearer access and refresh tokens.
    /// </returns>
    /// <response code="200">Returns the newly created access and refresh tokens.</response>
    /// <response code="400">If the refresh token is invalid or already used.</response>
    [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RefreshToken400ErrorResultExample))]
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = _requestToCommandMapper.Map(request);
        var result = await Mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return Failure(result, BadRequest);
        }

        UpdateUserContextToken(result.Value!.UserContextToken);
        var response = _resultToResponseMapper.Map(result.Value);

        return Ok(response);
    }

    /// <summary>
    /// Invalidates refresh token and user context for current user.
    /// </summary>
    /// <response code="200">Token is successfully invalidated.</response>
    /// <response code="400">If the user's access token is invalid.</response>
    /// <response code="401">If user context is missing.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LogoutToken400ErrorResultExample))]
    [Authorize]
    [HttpPost("[action]")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var email = User.GetEmailClaim();
        var command = _requestToCommandMapper.Map(email);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Failure(result, BadRequest);
        }
        
        RemoveUserContextToken();

        return Ok();
    }

    #region ContextTokenManagement

    [NonAction]
    private void UpdateUserContextToken(string token)
    {
        Response.Cookies.Append(CustomClaimNames.UserContext, token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
    }

    [NonAction]
    private void RemoveUserContextToken()
    {
        Response.Cookies.Delete(CustomClaimNames.UserContext);
    }

    #endregion 
}