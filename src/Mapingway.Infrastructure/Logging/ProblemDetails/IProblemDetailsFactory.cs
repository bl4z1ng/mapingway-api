using Mapingway.Application.Behaviors.Validation;
using Mapingway.Application.Contracts;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Infrastructure.Logging.ProblemDetails;

public interface IProblemDetailsFactory
{
    public Microsoft.AspNetCore.Mvc.ProblemDetails CreateFromError(
        Error error,
        string instance,
        int? statusCode = null,
        string? type = null,
        string? detail = null);

    public Microsoft.AspNetCore.Mvc.ProblemDetails CreateFromValidationError(
        ValidationError validationError,
        string instance,
        int? statusCode = null,
        string? type = null,
        string? detail = null);

    public Microsoft.AspNetCore.Mvc.ProblemDetails CreateFromHttpError(
        HttpError httpError,
        string instance,
        int? statusCode = null,
        string? type = null,
        string? detail = null);
}
