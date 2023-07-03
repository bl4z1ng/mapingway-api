using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(token => token.Id);

        builder
            .Property(token => token.Value)
            .IsRequired();

        builder.HasIndex(token => new { token.Value, token.UserId });

        builder
            .HasOne(token => token.User)
            .WithOne(user => user.RefreshToken);

        builder
            .HasOne(token => token.User)
            .WithMany(user => user.UsedRefreshTokens)
            .HasForeignKey(token => token.UserId);
    }
}