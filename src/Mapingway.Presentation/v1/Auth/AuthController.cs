using Mapingway.Application.Features.Auth.Login;
using Mapingway.Application.Features.Auth.Logout;
using Mapingway.Application.Features.Auth.Refresh;
using Mapingway.Infrastructure.Authentication.Claims;
using Mapingway.Presentation.Swagger.Examples;
using Mapingway.Presentation.v1.Auth.Requests;
using Mapingway.Presentation.v1.Auth.Responses;
using Mapingway.SharedKernel.Result;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.v1.Auth;

[Route(Routes.BasePath, Order = 1)]
public class AuthController : BaseApiController
{
    public AuthController(ISender sender, IMapper mapper) : base(sender, mapper) { }

    #region Metadata

    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <returns>
    /// A newly generated Bearer token.
    /// </returns>
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]

    #endregion
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AccessTokenResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var command = Mapper.Map<LoginCommand>(request);

        var result = await Sender.Send(command, ct);

        if (result.IsFailure)
        {
            return Error(result);
        }

        UpdateUserContextToken(result.Value!.UserContextToken);
        var response = Mapper.Map<AccessTokenResponse>(result.Value);

        return Ok(response);
    }

    #region Metadata

    /// <summary>
    /// Refreshes access and refresh tokens and invalidates passed refresh token.
    /// </summary>
    /// <returns>
    /// A newly generated Bearer access and refresh tokens.
    /// </returns>
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RefreshToken400ErrorResponseExample))]

    #endregion
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken ct)
    {
        var command = Mapper.Map<RefreshTokenCommand>(request);

        var result = await Sender.Send(command, ct);
        if (result.IsFailure) return Error(result);

        UpdateUserContextToken(result.Value!.UserContextToken);
        var response = Mapper.Map<AccessTokenResponse>(result.Value);

        return Ok(response);
    }

    #region Metadata

    /// <summary>
    /// Invalidates refresh token and user context for current user.
    /// </summary>
    /// <response code="200">Token is successfully invalidated.</response>
    /// <response code="400">If the user's access token is invalid.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(LogoutToken400ErrorResponseExample))]

    #endregion
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken ct)
    {
        var email = User.GetEmailClaim();
        if (email is null) return BadRequest("Authorization data is invalid, email was not found.");

        var command = Mapper.Map<LogoutCommand>((email, request.RefreshToken));

        var result = await Sender.Send(command, ct);
        if (result.IsFailure) return Error(result);

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
