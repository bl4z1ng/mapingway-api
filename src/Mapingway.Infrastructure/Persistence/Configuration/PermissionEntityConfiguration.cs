using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Mapingway.Infrastructure.Authentication.Permissions.Permission;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class PermissionEntityConfiguration : IEntityTypeConfiguration<Domain.Auth.Permission>
{
    public void Configure(EntityTypeBuilder<Domain.Auth.Permission> builder)
    {
        builder.ToTable("Permissions");
        
        builder.HasKey(permission => permission.Id);

        builder.Property(permission => permission.Name).IsRequired();

        var permissions = Enum.GetValues<Permission>()
            .Select(permission => new Domain.Auth.Permission
            {
                Id = (int)permission,
                Name = permission.ToString()
            });

        builder.HasData(permissions);
    }
}