using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UsedRefreshTokenFamilyRepository : GenericRepository<RefreshTokenFamily>, IUsedRefreshTokenFamilyRepository
{
    public UsedRefreshTokenFamilyRepository(DbContext context) : base(context)
    {
    }
}