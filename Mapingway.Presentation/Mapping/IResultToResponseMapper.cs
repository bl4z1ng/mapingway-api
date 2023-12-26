using Mapingway.Application.Contracts.Authentication.Results;
using Mapingway.Presentation.Controllers.Response;

namespace Mapingway.Presentation.Mapping;

public interface IResultToResponseMapper
{
    LoginResponse Map(AuthenticationResult result);
    RefreshResponse Map(RefreshTokenResult result);

}