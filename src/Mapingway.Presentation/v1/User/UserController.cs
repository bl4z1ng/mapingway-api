using Mapingway.Application.Features.User.Register;
using Mapingway.Infrastructure.Logging.ProblemDetails;
using Mapingway.Presentation.Shared;
using Mapingway.Presentation.Swagger.Examples;
using Mapingway.Presentation.v1.User.Requests;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.v1.User;

[Route(Routes.BasePath, Order = 2)]
public class UserController : BaseApiController
{
    public UserController(
        ISender sender,
        IMapper mapper,
        IProblemDetailsFactory factory) : base(sender, mapper, factory) { }

    #region Metadata

    /// <summary>
    /// Registers new user.
    /// </summary>
    /// <returns>
    /// Data about user registration and user details for caching.
    /// </returns>
    /// <response code="200">Token is successfully invalidated.</response>
    [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status422UnprocessableEntity, typeof(RegisterValidationErrors))]

    #endregion
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct = default)
    {
        var command = Mapper.Map<CreateUserCommand>(request);

        var result = await Sender.Send(command, ct);

        //TODO: split response and result
        return result.IsSuccess ? Ok(result.Value) : Problem(result);
    }
}
