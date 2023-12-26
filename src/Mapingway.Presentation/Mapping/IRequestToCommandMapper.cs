using Mapingway.Application.Features.Auth.Commands.Login;
using Mapingway.Application.Features.Auth.Commands.Logout;
using Mapingway.Application.Features.Auth.Commands.Refresh;
using Mapingway.Application.Features.Users.Commands.Register;
using Mapingway.Presentation.Controllers.Requests.Auth;
using Mapingway.Presentation.Controllers.Requests.User;

namespace Mapingway.Presentation.Mapping;

public interface IRequestToCommandMapper
{
    LoginCommand Map(LoginRequest request);
    CreateUserCommand Map(RegisterRequest request);
    RefreshTokenCommand Map(RefreshTokenRequest request);
    LogoutCommand Map(string email, string refreshToken);
}