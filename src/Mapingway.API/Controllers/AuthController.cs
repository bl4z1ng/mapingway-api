using System.Net.Mime;
using Mapingway.API.Internal;
using Mapingway.Application.Contracts.User.Request;
using Mapingway.Application.Users.Commands.Register;
using Mapingway.Application.Users.Commands.Login;
using Mapingway.Common.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Mapingway.API.Controllers;

[ApiController]
[ApiRoute("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : BaseApiController
{
    public AuthController(ILoggerFactory loggerFactory, IMediator mediator) : 
        base(loggerFactory, mediator, typeof(AuthController).ToString())
    {
    }

    /// <summary>
    /// Registers new user.
    /// </summary>
    /// <returns>
    /// JSON with data about user registration and user data for caching.
    /// </returns>
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        //add mapping
        var command = new CreateUserCommand(
            request.Email, 
            request.Password, 
            request.FirstName, 
            request.LastName);

        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Authenticates registered user.
    /// </summary>
    /// <returns>
    /// A newly generated Bearer token.
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        //add mapping
        var command = new LoginCommand(request.Email, request.Password);

        var result = await Mediator.Send(command, cancellationToken);
        
        //work out multiple errors
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }
}