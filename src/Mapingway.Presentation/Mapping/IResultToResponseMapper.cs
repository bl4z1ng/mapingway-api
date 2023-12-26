using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Presentation.Controllers.Response;

namespace Mapingway.Presentation.Mapping;

public interface IResultToResponseMapper
{
    LoginResponse Map(AuthenticationResult result);
    RefreshResponse Map(RefreshTokenResult result);

}