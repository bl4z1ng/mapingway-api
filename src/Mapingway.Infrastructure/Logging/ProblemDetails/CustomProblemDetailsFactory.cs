using Mapingway.Application.Behaviors.Validation;
using Mapingway.Application.Contracts;
using Mapingway.SharedKernel.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Mapingway.Infrastructure.Logging.ProblemDetails;

public class CustomProblemDetailsFactory : IProblemDetailsFactory
{
    public Microsoft.AspNetCore.Mvc.ProblemDetails CreateFromError(
        Error error,
        string instance,
        int? statusCode = null,
        string? type = null,
        string? detail = null)
    {
        statusCode ??= ProblemDetailsDefaults.DefaultStatusCode;
        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Instance = instance,
            Status = statusCode,
            Type = type ?? ProblemDetailsDefaults.StatusCodesUrl + statusCode,
            Detail = detail ?? error.Message ?? ReasonPhrases.GetReasonPhrase(statusCode.Value)
        };
    }

    public Microsoft.AspNetCore.Mvc.ProblemDetails CreateFromValidationError(
        ValidationError validationError,
        string instance,
        int? statusCode = null,
        string? type = null,
        string? detail = null)
    {
        statusCode ??= ProblemDetailsDefaults.ValidationStatusCode;
        return new ValidationProblemDetails
        {
            Instance = instance,
            Status = statusCode,
            Type = type ?? ProblemDetailsDefaults.StatusCodesUrl + statusCode,
            Detail = detail ?? "One or more validation errors occured.",
            Errors = validationError.Failures
        };
    }

    public Microsoft.AspNetCore.Mvc.ProblemDetails CreateFromHttpError(
        HttpError httpError,
        string instance,
        int? statusCode = null,
        string? type = null,
        string? detail = null)
    {
        return CreateFromError(
            httpError,
            instance,
            statusCode: statusCode ?? httpError.ResponseStatusCode,
            type: type,
            detail: detail);
    }
}

public class ProblemDetailsDefaults
{
    public const string TraceIdProperty = "traceId";
    public const string StatusCodesUrl = "https://httpstatuses.io/";
    public const int DefaultStatusCode = StatusCodes.Status500InternalServerError;
    public const int ValidationStatusCode = StatusCodes.Status422UnprocessableEntity;
}
