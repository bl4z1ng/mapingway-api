﻿using Mapingway.Common.Result;
using Mapingway.Common.Result.Validation;
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


    [NonAction]
    protected IActionResult HandleFailure(Result result, FailureResultDelegate generateFailureResult)
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


    protected delegate IActionResult FailureResultDelegate(object? value);


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