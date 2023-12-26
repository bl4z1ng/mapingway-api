using Mapingway.Application.Features.Auth.Login;
using Mapingway.Application.Features.Auth.Logout;
using Mapingway.Application.Features.Auth.Refresh;
using Mapingway.Application.Features.User.Register;
using Mapingway.Presentation.Controllers.Requests.Auth;
using Mapingway.Presentation.Controllers.Requests.User;
using Riok.Mapperly.Abstractions;

namespace Mapingway.Presentation.Mapping;

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