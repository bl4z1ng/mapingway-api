using System.Net.Mime;
using Mapingway.API.Extensions;
using Mapingway.API.Internal;
using Mapingway.API.Internal.Mapping;
using Mapingway.API.Swagger.Examples.Results.Token;
using Mapingway.Application.Contracts.Token.Request;
using Mapingway.Application.Contracts.Token.Result;
using Mapingway.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Controllers;

[ApiRoute("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TokenController: BaseApiController
{
    private readonly IMapper _mapper;


    public TokenController(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper) 
        : base(loggerFactory, mediator, typeof(TokenController).ToString())
    {
        _mapper = mapper;
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
        var command = _mapper.Map(request);

        var result = await Mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result, BadRequest);
    }

    /// <summary>
    /// Invalidates refresh token for current user.
    /// </summary>
    /// <response code="200">Token is successfully invalidated</response>
    /// <response code="400">If the user's access token is invalid</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RevokeToken400ErrorResultExample))]
    [Authorize]
    [HttpPost("[action]")]
    public async Task<IActionResult> Revoke(CancellationToken cancellationToken)
    {
        var email = User.GetEmailClaim();
        var command = _mapper.Map(email);

        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : HandleFailure(result, BadRequest);
    }
}