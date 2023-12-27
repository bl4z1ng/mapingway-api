using System.Net.Mime;
using Mapingway.SharedKernel.Result;
using Mapingway.SharedKernel.ValidationResult;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.Presentation.v1;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class BaseApiController : ControllerBase
{
    protected readonly ISender Sender;

    public BaseApiController(ISender sender)
    {
        Sender = sender;
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

    protected delegate IActionResult FailureResultDelegate(object? value);
}
