using Mapingway.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.API.Controllers;

[Route("User")]
public class UserController : BaseApiController
{
    public UserController(ILoggerFactory loggerFactory, IMediator mediator) : 
        base(loggerFactory, mediator, typeof(UserController).ToString())
    {
    }
}