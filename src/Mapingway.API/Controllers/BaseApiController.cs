using System.Reflection.Emit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.API.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public BaseApiController(ILoggerFactory loggerFactory, IMediator mediator, string controllerType = "BaseController")
    {
        _logger = loggerFactory.CreateLogger(controllerType);
        _mediator = mediator;
    }
}