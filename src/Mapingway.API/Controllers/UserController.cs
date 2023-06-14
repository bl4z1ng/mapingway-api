using Mapingway.Application.Users.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Register(CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            Email: "userDto@gmail.com",
            Password: "123",
            Role: "Jija",
            FirstName: "FirstName",
            LastName: "LastName");
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}