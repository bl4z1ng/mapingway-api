using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Features.Auth.Login;
using Mapingway.Presentation.v1.Auth.Responses;

namespace Mapingway.Presentation.Mapping;

public interface IResultToResponseMapper
{
    AccessTokenResponse Map(LoginResult result);
    AccessTokenResponse Map(RefreshTokenResult result);
}
