using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Features.Auth.Login;
using Mapingway.Presentation.v1.Auth.Responses;

namespace Mapingway.Presentation.Mapping;

public interface IResultToResponseMapper
{
    LoginResponse Map(LoginResult result);
    RefreshResponse Map(RefreshTokenResult result);
}