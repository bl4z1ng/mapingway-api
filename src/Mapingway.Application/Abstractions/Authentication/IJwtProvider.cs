using Mapingway.Common.Permission;
using Mapingway.Domain.User;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateToken(User user, IEnumerable<string> permissions);
}