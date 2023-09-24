using Mapingway.API.Internal.Response;
using Mapingway.Application.Contracts.Auth;
using Riok.Mapperly.Abstractions;

namespace Mapingway.API.Internal.Mapping;

[Mapper]
public partial class MapperlyResultToResponseMapper : IResultToResponseMapper
{
    public partial LoginResponse Map(AuthenticationResult result);
    public partial RefreshResponse Map(RefreshTokenResult result);

}