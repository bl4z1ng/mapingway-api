using Mapingway.Application.Auth.Commands.Login;
using Mapingway.Application.Auth.Commands.Logout;
using Mapingway.Application.Auth.Commands.Refresh;
using Mapingway.Application.Contracts.Auth.Request;
using Mapingway.Application.Contracts.User.Request;
using Mapingway.Application.Users.Commands.Register;
using Riok.Mapperly.Abstractions;

namespace Mapingway.API.Internal.Mapping;

[Mapper]
public partial class MapperlyRequestToCommandMapper : IRequestToCommandMapper
{
    public partial LoginCommand Map(LoginRequest request);
    public partial CreateUserCommand Map(RegisterRequest request);
    public partial RefreshTokenCommand Map(RefreshTokenRequest request);
    public partial LogoutTokenCommand Map(string? email);
}