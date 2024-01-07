using Mapingway.Application.Features.Auth.Login;
using Mapingway.Application.Features.Auth.Logout;
using Mapingway.Application.Features.Auth.Refresh;
using Mapingway.Infrastructure.Authentication.Claims;
using Mapingway.Infrastructure.Authentication.Token.Parser;
using Mapingway.Infrastructure.Logging.ProblemDetails;
using Mapingway.Presentation.Shared;
using Mapingway.Presentation.Swagger.Examples;
using Mapingway.Presentation.Swagger.Filters.Utility;
using Mapingway.Presentation.v1.Auth.Requests;
using Mapingway.Presentation.v1.Auth.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.v1.Auth;

[Route(Routes.BasePath, Order = 1)]
public class AuthController : BaseApiController
{
    public AuthController(
        ISender sender,
        IMapper mapper,
        IProblemDetailsFactory factory) : base(sender, mapper, factory) {}

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
        if (result.IsFailure) return Problem(result);

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
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, Type = typeof(ExampleProblemDetails), Description = "If provided token is invalid JWT Bearer.")]
    [SwaggerResponseExample(StatusCodes.Status422UnprocessableEntity, typeof(InvalidTokenValidationErrors))]

    #endregion
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(
        [FromBody]RefreshTokenRequest request,
        [FromServices] IJwtTokenParser tokenParser,
        CancellationToken ct)
    {
        var userEmail = tokenParser
            .GetPrincipalFromBearer(request.ExpiredToken, tokenExpired: true).GetEmailClaim();
        if (userEmail is null)
        {
            RemoveUserContextToken();
            return Unauthorized("Provided access token is not valid anymore, please, log-in again.");
        }

        var command = new RefreshTokenCommand(userEmail, request.RefreshToken);

        var result = await Sender.Send(command, ct);
        if (result.IsFailure) return Problem(result);

        UpdateUserContextToken(result.Value!.UserContextToken);
        var response = Mapper.Map<AccessTokenResponse>(result.Value);

        return Ok(response);
    }

    #region Metadata

    /// <summary>
    /// Invalidates refresh token and user context for current user.
    /// </summary>
    /// <response code="200">Token is successfully invalidated.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]

    #endregion
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken ct)
    {
        var email = User.GetEmailClaim();
        if (email is null)
        {
            RemoveUserContextToken();
            return Unauthorized("Authorization data is invalid, email was not found.");
        }

        var command = Mapper.Map<LogoutCommand>((email, request.RefreshToken));

        var result = await Sender.Send(command, ct);
        if (result.IsFailure) return Problem(result);

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
