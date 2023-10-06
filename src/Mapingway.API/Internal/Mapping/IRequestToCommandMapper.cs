using Mapingway.API.Controllers.Requests.Auth;
using Mapingway.API.Controllers.Requests.User;
using Mapingway.Application.Auth.Commands.Login;
using Mapingway.Application.Auth.Commands.Logout;
using Mapingway.Application.Auth.Commands.Refresh;
using Mapingway.Application.Users.Commands.Register;

namespace Mapingway.API.Internal.Mapping;

public interface IRequestToCommandMapper
{
    LoginCommand Map(LoginRequest request);
    CreateUserCommand Map(RegisterRequest request);
    RefreshTokenCommand Map(RefreshTokenRequest request);
    LogoutCommand Map(string email, string refreshToken);
}