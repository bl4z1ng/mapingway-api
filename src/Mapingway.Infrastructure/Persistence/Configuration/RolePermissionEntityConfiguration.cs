using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Mapingway.Infrastructure.Authentication.Permissions.Permission;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        var rolePermissions = new List<RolePermission>();
        rolePermissions.AddRange(Create(Role.User, new List<Permission>
        {
            Permission.ReadUser
        }));
        rolePermissions.AddRange(Create(Role.Admin, new List<Permission>
        {
            Permission.ReadUser,
            Permission.UpdateUser,
            Permission.DeleteUser
        }));

        builder.HasData(rolePermissions);
    }


    private static IEnumerable<RolePermission> Create(Role role, IEnumerable<Permission> permissions)
    {
        return permissions.Select(permission =>
            new RolePermission
            {
                RoleId = role.Id,
                PermissionId = (int)permission
            })
            .ToList();
    }
}