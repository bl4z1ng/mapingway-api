using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.HasData(Create(-1, new List<Role> { Role.Admin }));
    }
    
    private static IEnumerable<UserRole> Create(int userId, IEnumerable<Role> roles)
    {
        return roles.Select(role => 
                new UserRole
                {
                    RoleId = role.Id, 
                    UserId = userId
                })
            .ToList();
    }
}