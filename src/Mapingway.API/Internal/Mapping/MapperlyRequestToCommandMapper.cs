using Mapingway.Application.Contracts.Token.Request;
using Mapingway.Application.Contracts.User.Request;
using Mapingway.Application.Tokens.Commands.Refresh;
using Mapingway.Application.Tokens.Commands.Revoke;
using Mapingway.Application.Users.Commands.Login;
using Mapingway.Application.Users.Commands.Register;
using Riok.Mapperly.Abstractions;

namespace Mapingway.API.Internal.Mapping;

[Mapper]
public partial class MapperlyRequestToCommandMapper : IRequestToCommandMapper
{
    public partial LoginCommand Map(LoginRequest request);
    public partial CreateUserCommand Map(RegisterRequest request);
    public partial RefreshTokenCommand Map(RefreshTokenRequest request);
    public partial RevokeTokenCommand Map(string? email);
}