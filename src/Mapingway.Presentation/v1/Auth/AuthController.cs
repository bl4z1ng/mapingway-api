using Mapingway.Infrastructure.Authentication.Claims;
using Mapingway.Presentation.Mapping;
using Mapingway.Presentation.Swagger.Examples.Results.Auth;
using Mapingway.Presentation.v1.Auth.Requests;
using Mapingway.Presentation.v1.Auth.Responses;
using Mapingway.SharedKernel.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.v1.Auth;

[Route(Routes.BasePath, Order = 1)]
public class AuthController : BaseApiController
{
    private readonly IRequestToCommandMapper _requestToCommandMapper;
    private readonly IResultToResponseMapper _resultToResponseMapper;

    public AuthController(
        ISender sender,
        IRequestToCommandMapper requestToCommandMapper,
        IResultToResponseMapper resultToResponseMapper) : base(sender)
    {
        _requestToCommandMapper = requestToCommandMapper;
        _resultToResponseMapper = resultToResponseMapper;
    }

    #region Metadata

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

    #endregion
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (User.Identity is { IsAuthenticated: true })
            return Conflict("User is already authenticated.");

        var command = _requestToCommandMapper.Map(request);
        var result = await Sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Failure(result, Unauthorized);
        }

        UpdateUserContextToken(result.Value!.UserContextToken);
        var response = _resultToResponseMapper.Map(result.Value);

        return Ok(response);
    }

    #region Metadata

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

    #endregion
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = _requestToCommandMapper.Map(request);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Failure(result, BadRequest);
        }

        UpdateUserContextToken(result.Value!.UserContextToken);
        var response = _resultToResponseMapper.Map(result.Value);

        return Ok(response);
    }

    #region Metadata

    /// <summary>
    /// Invalidates refresh token and user context for current user.
    /// </summary>
    /// <response code="200">Token is successfully invalidated.</response>
    /// <response code="400">If the user's access token is invalid.</response>
    /// <response code="401">If user context is missing.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LogoutToken400ErrorResultExample))]

    #endregion
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken cancellationToken)
    {
        var email = User.GetEmailClaim();
        if (email is null)
        {
            return BadRequest("Authorization data is invalid.");
        }

        var command = _requestToCommandMapper.Map(email, request.RefreshToken);
        var result = await Sender.Send(command, cancellationToken);

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
        Response.Cookies.Append(CustomClaims.UserContext, token,
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
        Response.Cookies.Delete(CustomClaims.UserContext);
    }

    #endregion
}
