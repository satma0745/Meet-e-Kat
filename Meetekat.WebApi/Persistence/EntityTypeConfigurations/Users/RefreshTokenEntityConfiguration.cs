namespace Meetekat.WebApi.Persistence.EntityTypeConfigurations.Users;

using Meetekat.WebApi.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshTokenEntity)
    {
        refreshTokenEntity.ToTable("refresh_tokens");

        refreshTokenEntity
            .HasKey(refreshToken => refreshToken.TokenId)
            .HasName("pk_refresh_tokens");

        refreshTokenEntity
            .HasIndex(refreshToken => refreshToken.UserId)
            .HasDatabaseName("ix_refresh_tokens_user_id");

        refreshTokenEntity
            .Property(refreshToken => refreshToken.TokenId)
            .HasColumnName("token_id");

        refreshTokenEntity
            .Property(refreshToken => refreshToken.UserId)
            .HasColumnName("user_id");

        refreshTokenEntity
            .HasOne<User>()
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .HasConstraintName("fk_users_refresh_tokens_user_id");
    }
}
