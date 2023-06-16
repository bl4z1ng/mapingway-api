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
    public async Task<IActionResult> Register([FromBody] CreateUserCommand requestCommand, CancellationToken cancellationToken)
    {
        //add mapping
        var command = requestCommand;
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginCommand requestCommand, CancellationToken cancellationToken)
    {
        //add mapping
        var command = requestCommand;

        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }
}