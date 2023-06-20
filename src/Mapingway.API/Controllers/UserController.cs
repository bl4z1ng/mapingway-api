using Mapingway.Application.Contracts.User;
using Mapingway.Application.Users.Commands.Register;
using Mapingway.Application.Users.Commands.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Mapingway.API.Controllers;

[Route("[controller]")]
public class UserController : BaseApiController
{
    public UserController(ILoggerFactory loggerFactory, IMediator mediator) : 
        base(loggerFactory, mediator, typeof(UserController).ToString())
    {
    }
    
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

    [Authorize]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetUserById([FromQuery] GetUserRequest request, CancellationToken cancellationToken)
    {
        return Ok("User Access Granted");
    }
}