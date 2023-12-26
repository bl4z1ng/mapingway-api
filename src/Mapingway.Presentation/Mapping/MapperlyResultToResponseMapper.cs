using Mapingway.Application.Contracts.Authentication.Results;
using Mapingway.Presentation.Controllers.Response;
using Riok.Mapperly.Abstractions;

namespace Mapingway.Presentation.Mapping;

[Mapper]
public partial class MapperlyResultToResponseMapper : IResultToResponseMapper
{
    public partial LoginResponse Map(AuthenticationResult result);
    public partial RefreshResponse Map(RefreshTokenResult result);
}