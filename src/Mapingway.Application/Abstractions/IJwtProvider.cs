using Mapingway.Common.Permission;
using Mapingway.Domain.User;

namespace Mapingway.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user, IEnumerable<Permissions> permissions);
}