using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UsedRefreshTokenFamilyRepository : GenericRepository<RefreshTokenFamily>
{
    public UsedRefreshTokenFamilyRepository(DbContext context) : base(context)
    {
    }
}
