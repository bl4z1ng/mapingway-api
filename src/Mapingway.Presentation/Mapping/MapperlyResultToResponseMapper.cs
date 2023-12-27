using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Application.Features.Auth.Login;
using Mapingway.Presentation.v1.Auth.Responses;
using Riok.Mapperly.Abstractions;

namespace Mapingway.Presentation.Mapping;

[Mapper]
public partial class MapperlyResultToResponseMapper : IResultToResponseMapper
{
    public partial LoginResponse Map(LoginResult result);
    public partial RefreshResponse Map(RefreshTokenResult result);
}