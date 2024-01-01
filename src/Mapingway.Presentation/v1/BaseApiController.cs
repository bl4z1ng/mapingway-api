using System.Net.Mime;
using Mapingway.Application.Behaviors.Validation;
using Mapingway.SharedKernel.Result;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mapingway.Presentation.v1;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class BaseApiController : ControllerBase
{
    protected readonly IMapper Mapper;
    protected readonly ISender Sender;

    public BaseApiController(ISender sender, IMapper mapper)
    {
        Sender = sender;
        Mapper = mapper;
    }

    [NonAction]
    protected ActionResult Error(Result result, FailureResultDelegate? problem = null)
    {
        if ( result.IsSuccess ) throw new InvalidOperationException();

        //TODO: transform error to problem details
        if ( problem is not null ) return problem(result.Error);

        return result switch
        {
            IValidationResult validationError => UnprocessableEntity(
                new ValidationProblemDetails
                {
                    Errors = validationError.Failures
                }),
            _ => BadRequest(result.Error)
        };
    }

    protected delegate ActionResult FailureResultDelegate(object? value);
}
