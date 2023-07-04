using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class RefreshTokenFamilyEntityConfiguration : IEntityTypeConfiguration<RefreshTokenFamily>
{
    public void Configure(EntityTypeBuilder<RefreshTokenFamily> builder)
    {
        builder.ToTable("RefreshTokenFamilies");
        builder.HasKey(family => family.Id);

        builder.HasIndex(family => family.UserId);

        builder
            .HasOne(family => family.User)
            .WithOne(user => user.UsedRefreshTokensFamily);
    }
}