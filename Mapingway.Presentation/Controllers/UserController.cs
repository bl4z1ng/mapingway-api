using Mapingway.Application.Contracts.User;
using Mapingway.Common.Result;
using Mapingway.Presentation.Controllers.Requests.User;
using Mapingway.Presentation.Mapping;
using Mapingway.Presentation.Swagger.Documentation;
using Mapingway.Presentation.Swagger.Examples.Results.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Controllers;

[SwaggerControllerOrder(0)]
public class UserController : BaseApiController
{
    private readonly IRequestToCommandMapper _requestToCommandMapper;

    public UserController(ILoggerFactory loggerFactory, IRequestToCommandMapper requestToCommandMapper, IMediator mediator) 
        : base(loggerFactory, mediator, typeof(UserController).ToString())
    {
        _requestToCommandMapper = requestToCommandMapper;
    }


    #region Metadata

    /// <summary>
    /// Registers new user.
    /// </summary>
    /// <returns>
    /// Data about user registration and user details for caching.
    /// </returns>
    /// <response code="200">User is successfully registered.</response>
    /// <response code="400">If user data is invalid.</response>
    [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(Register400ErrorResultExample))]

    #endregion
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = _requestToCommandMapper.Map(request);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : Failure(result, BadRequest);
    }
}