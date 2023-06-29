using Mapingway.Common.Repository;
using Mapingway.Domain.Auth;
using Mapingway.Domain.User;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
}