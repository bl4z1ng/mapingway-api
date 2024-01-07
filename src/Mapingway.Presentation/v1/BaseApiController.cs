using System.Net.Mime;
using Mapingway.Application.Behaviors.Validation;
using Mapingway.Application.Contracts;
using Mapingway.Infrastructure.Logging.ProblemDetails;
using Mapingway.SharedKernel.Result;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.Presentation.v1;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class BaseApiController : ControllerBase
{
    private readonly IProblemDetailsFactory _factory;
    protected readonly IMapper Mapper;
    protected readonly ISender Sender;
    private readonly IProblemDetailsFactory _factory;

    public BaseApiController(ISender sender, IMapper mapper, IProblemDetailsFactory factory)
    {
        Sender = sender;
        Mapper = mapper;
        _factory = factory;
    }

    [NonAction]
    protected ActionResult Problem(Result result, Func<object?, ActionResult>? problem = null)
    {
        if (result.IsSuccess) throw new InvalidOperationException();

        var instance = HttpContext.Request.Path.ToUriComponent();

        if (problem is not null)
        {
            var problemDetails = _factory.CreateFromError(result.Error, instance, detail: result.Error.Message);
            return problem(problemDetails);
        }

        return result.Error switch
        {
            ValidationError validationError => UnprocessableEntity(_factory.CreateFromValidationError(validationError, instance)),
            HttpError httpError => StatusCode(httpError.ResponseStatusCode, _factory.CreateFromHttpError(httpError, instance)),
            _ => BadRequest(_factory.CreateFromError(result.Error, instance, detail: result.Error.Message))
        };
    }
}
