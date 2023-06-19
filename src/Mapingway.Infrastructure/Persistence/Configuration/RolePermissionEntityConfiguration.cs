using Mapingway.Common.Permission;
using Mapingway.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class RolePermissionEntityConfiguration: IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        // builder.HasData(
        //     Create(Role.User, new List<Permissions>
        //     {
        //         Permissions.ReadUser
        //     }),
        //     Create(Role.Admin, new List<Permissions>
        //     {
        //         Permissions.ReadUser,
        //         Permissions.UpdateUser,
        //         Permissions.DeleteUser
        //     })
        // );
    }


    private static IEnumerable<RolePermission> Create(Role role, IEnumerable<Permissions> permissions)
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