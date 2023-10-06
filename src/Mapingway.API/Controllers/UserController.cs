using System.Net.Mime;
using Mapingway.API.Controllers.Requests.User;
using Mapingway.API.Internal;
using Mapingway.API.Internal.Mapping;
using Mapingway.API.Swagger.Documentation;
using Mapingway.API.Swagger.Examples.Results.User;
using Mapingway.Application.Contracts.User;
using Mapingway.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Controllers;

[ApiRoute("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerControllerOrder(0)]
public class UserController : BaseApiController
{
    private readonly IRequestToCommandMapper _requestToCommandMapper;


    public UserController(ILoggerFactory loggerFactory, IRequestToCommandMapper requestToCommandMapper, IMediator mediator) 
        : base(loggerFactory, mediator, typeof(UserController).ToString())
    {
        _requestToCommandMapper = requestToCommandMapper;
    }


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
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = _requestToCommandMapper.Map(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : Failure(result, BadRequest);
    }
}