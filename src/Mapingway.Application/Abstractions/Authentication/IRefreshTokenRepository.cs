using Mapingway.Common.Repository;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
}