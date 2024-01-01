using Mapingway.Application.Features.User.Register;
using Mapingway.Presentation.Swagger.Examples;
using Mapingway.Presentation.v1.User.Requests;
using Mapingway.SharedKernel.Result;
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
    public UserController(ISender sender, IMapper mapper) : base(sender, mapper) { }

    #region Metadata

    /// <summary>
    /// Registers new user.
    /// </summary>
    /// <returns>
    /// Data about user registration and user details for caching.
    /// </returns>
    /// <response code="400">If user data is invalid.</response>
    //TODO: split response and result
    [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(Register400ErrorResultExample))]

    #endregion
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct = default)
    {
        var command = Mapper.Map<CreateUserCommand>(request);

        var result = await Sender.Send(command, ct);

        //TODO: split response and result
        return result.IsSuccess ? Ok(result.Value) : Error(result);
    }
}
