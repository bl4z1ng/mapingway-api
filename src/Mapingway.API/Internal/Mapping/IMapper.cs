using Mapingway.Application.Contracts.Token.Request;
using Mapingway.Application.Contracts.User.Request;
using Mapingway.Application.Tokens.Commands.Refresh;
using Mapingway.Application.Tokens.Commands.Revoke;
using Mapingway.Application.Users.Commands.Login;
using Mapingway.Application.Users.Commands.Register;

namespace Mapingway.API.Internal.Mapping;

public interface IMapper
{
    LoginCommand Map(LoginRequest request);
    CreateUserCommand Map(RegisterRequest request);
    RefreshTokenCommand Map(RefreshTokenRequest request);
    RevokeTokenCommand Map(string? email);
}