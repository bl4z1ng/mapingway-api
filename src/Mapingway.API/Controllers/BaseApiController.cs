using Mapingway.Application.Behaviors;
using Mapingway.Common.Result;
using Mapingway.Common.ValidationResult;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.API.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected delegate IActionResult FailureResultDelegate(object? value);
    protected readonly ILogger Logger;
    protected readonly IMediator Mediator;

    public BaseApiController(ILoggerFactory loggerFactory, IMediator mediator, string controllerType = "BaseController")
    {
        Mediator = mediator;
        Logger = loggerFactory.CreateLogger(controllerType);
    }


    [NonAction]
    protected IActionResult Failure(Result result, FailureResultDelegate generateFailureResult)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return result.IsValidationResult()
            ? BadRequest(
            CreateProblemDetails(
                "Validation Error",
                StatusCodes.Status400BadRequest,
                result.Error,
                (result as IValidationResult)!.Errors))
            : generateFailureResult(result.Error);
    }


    private static ProblemDetails CreateProblemDetails(
        string title, 
        int status, 
        Error error, 
        Error[]? errors = null)
    {
        return new ProblemDetails
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
    }
}