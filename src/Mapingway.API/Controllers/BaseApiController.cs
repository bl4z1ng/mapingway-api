using System.Reflection.Emit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.API.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected readonly ILogger Logger;
    protected readonly IMediator Mediator;

    public BaseApiController(ILoggerFactory loggerFactory, IMediator mediator, string controllerType = "BaseController")
    {
        Mediator = mediator;
        Logger = loggerFactory.CreateLogger(controllerType);
    }
}