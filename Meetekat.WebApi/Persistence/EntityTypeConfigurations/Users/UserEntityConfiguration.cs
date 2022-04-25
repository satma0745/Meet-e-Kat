namespace Meetekat.WebApi.Persistence.EntityTypeConfigurations.Users;

using Meetekat.WebApi.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userEntity)
    {
        userEntity.ToTable("users");

        userEntity
            .HasKey(user => user.Id)
            .HasName("pk_users");

        userEntity
            .HasIndex(user => user.Username)
            .IsUnique()
            .HasDatabaseName("ux_users_username");

        userEntity
            .Property(user => user.Id)
            .HasColumnName("id");

        userEntity
            .Property(user => user.Username)
            .IsRequired()
            .HasColumnName("username");

        userEntity
            .Property(user => user.Password)
            .IsRequired()
            .HasColumnName("password");
        
        userEntity
            .HasDiscriminator<string>("role")
            .HasValue<Guest>(nameof(Guest))
            .HasValue<Organizer>(nameof(Organizer));
    }
}
