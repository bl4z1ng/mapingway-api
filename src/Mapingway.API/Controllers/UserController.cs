using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.API.Controllers;

[Route("User")]
public class UserController : BaseApiController
{
    public UserController(ILoggerFactory loggerFactory, IMediator mediator) : base(loggerFactory, mediator, typeof(UserController).ToString())
    {
    }
}