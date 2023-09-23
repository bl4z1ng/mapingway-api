using Mapingway.API.Internal.Contracts;
using Mapingway.Application.Contracts.Auth.Result;
using Riok.Mapperly.Abstractions;

namespace Mapingway.API.Internal.Mapping;

[Mapper]
public partial class MapperlyResultToResponseMapper : IResultToResponseMapper
{
    public partial LoginResponse Map(AuthenticationResult result);
}