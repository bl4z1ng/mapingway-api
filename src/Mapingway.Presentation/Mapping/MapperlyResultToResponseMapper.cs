using Mapingway.Application.Contracts;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Presentation.Controllers.Response;
using Riok.Mapperly.Abstractions;

namespace Mapingway.Presentation.Mapping;

[Mapper]
public partial class MapperlyResultToResponseMapper : IResultToResponseMapper
{
    public partial LoginResponse Map(AuthenticationResult result);
    public partial RefreshResponse Map(RefreshTokenResult result);
}