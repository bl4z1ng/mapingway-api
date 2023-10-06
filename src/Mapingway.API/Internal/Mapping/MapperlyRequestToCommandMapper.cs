using Mapingway.API.Controllers.Requests.Auth;
using Mapingway.API.Controllers.Requests.User;
using Mapingway.Application.Auth.Commands.Login;
using Mapingway.Application.Auth.Commands.Logout;
using Mapingway.Application.Auth.Commands.Refresh;
using Mapingway.Application.Users.Commands.Register;
using Riok.Mapperly.Abstractions;

namespace Mapingway.API.Internal.Mapping;

[Mapper]
public partial class MapperlyRequestToCommandMapper : IRequestToCommandMapper
{
    public partial LoginCommand Map(LoginRequest request);
    public partial CreateUserCommand Map(RegisterRequest request);
    public partial RefreshTokenCommand Map(RefreshTokenRequest request);

    public LogoutCommand Map(string email, string refreshToken)
    {
        return new LogoutCommand(email, refreshToken);
    }
}