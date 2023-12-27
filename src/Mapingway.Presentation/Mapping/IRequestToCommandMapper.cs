using Mapingway.Application.Features.Auth.Login;
using Mapingway.Application.Features.Auth.Logout;
using Mapingway.Application.Features.Auth.Refresh;
using Mapingway.Application.Features.User.Register;
using Mapingway.Presentation.v1.Auth.Requests;
using Mapingway.Presentation.v1.User.Requests;

namespace Mapingway.Presentation.Mapping;

public interface IRequestToCommandMapper
{
    LoginCommand Map(LoginRequest request);
    CreateUserCommand Map(RegisterRequest request);
    RefreshTokenCommand Map(RefreshTokenRequest request);
    LogoutCommand Map(string email, string refreshToken);
}