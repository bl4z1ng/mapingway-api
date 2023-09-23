using Mapingway.Application.Contracts.Auth.Request;
using Mapingway.Application.Contracts.User.Request;
using Mapingway.Application.Tokens.Commands.Refresh;
using Mapingway.Application.Tokens.Commands.Logout;
using Mapingway.Application.Users.Commands.Login;
using Mapingway.Application.Users.Commands.Register;

namespace Mapingway.API.Internal.Mapping;

public interface IRequestToCommandMapper
{
    LoginCommand Map(LoginRequest request);
    CreateUserCommand Map(RegisterRequest request);
    RefreshTokenCommand Map(RefreshTokenRequest request);
    LogoutTokenCommand Map(string? email);
}