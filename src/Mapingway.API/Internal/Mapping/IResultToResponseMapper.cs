using Mapingway.API.Internal.Response;
using Mapingway.Application.Contracts.Auth;

namespace Mapingway.API.Internal.Mapping;

public interface IResultToResponseMapper
{
    LoginResponse Map(AuthenticationResult result);
    RefreshResponse Map(RefreshTokenResult result);

}