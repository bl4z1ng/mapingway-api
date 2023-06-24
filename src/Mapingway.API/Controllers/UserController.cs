using System.Net.Mime;
using Mapingway.Application.Contracts.User;
using Mapingway.Application.Users.Commands.Register;
using Mapingway.Application.Users.Commands.Login;
using Mapingway.Common.Permission;
using Mapingway.Common.Result;
using Mapingway.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Mapingway.API.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Route("[controller]")]
public class UserController : BaseApiController
{
    public UserController(ILoggerFactory loggerFactory, IMediator mediator) : 
        base(loggerFactory, mediator, typeof(UserController).ToString())
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

    [HasPermission(Permissions.UpdateUser)]
    [HttpGet("{userId:int?}")]
    public async Task<IActionResult> GetUserById(int userId, CancellationToken cancellationToken)
    {
        return Ok($"User {userId} gets Access");
    }
}