using System.Net.Mime;
using Mapingway.API.Internal;
using Mapingway.API.Internal.Mapping;
using Mapingway.API.Swagger.Examples.Results.User;
using Mapingway.Application.Contracts.User.Request;
using Mapingway.Application.Contracts.User.Result;
using Mapingway.Common.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Controllers;

[ApiRoute("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : BaseApiController
{
    private readonly IMapper _mapper;


    public AuthController(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper) : 
        base(loggerFactory, mediator, typeof(AuthController).ToString())
    {
        _mapper = mapper;
    }


    /// <summary>
    /// Registers new user.
    /// </summary>
    /// <returns>
    /// JSON with data about user registration and user data for caching.
    /// </returns>
    /// <response code="200">User is successfully registered</response>
    /// <response code="400">If user data is invalid</response>
    [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(Register400ErrorResultExample))]
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map(request);

        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Authenticates registered user.
    /// </summary>
    /// <returns>
    /// A newly generated Bearer token.
    /// </returns>
    /// <response code="200">User is successfully logged in</response>
    /// <response code="401">If user credentials are not valid</response>
    /// <response code="409">If the user is already authenticated</response>
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
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

        var command = _mapper.Map(request);

        var result = await Mediator.Send(command, cancellationToken);

        // TODO: work out multiple errors and validation errors
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }
}