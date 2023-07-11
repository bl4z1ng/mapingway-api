using Mapingway.Common.Interfaces;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
}