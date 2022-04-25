namespace Meetekat.WebApi.Persistence.EntityTypeConfigurations.Users;

using System.Collections.Generic;
using Meetekat.WebApi.Entities;
using Meetekat.WebApi.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GuestEntityConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> guestEntity) =>
        guestEntity
            .HasMany(guest => guest.MeetupsSignedUpFor)
            .WithMany(meetup => meetup.SignedUpGuests)
            .UsingEntity<Dictionary<string, object>>(
                joinEntity => joinEntity
                    .HasOne<Meetup>()
                    .WithMany()
                    .HasForeignKey("meetup_id")
                    .HasConstraintName("fk_meetups_meetups_users_signups_meetup_id"),
                joinEntity => joinEntity
                    .HasOne<Guest>()
                    .WithMany()
                    .HasForeignKey("guest_id")
                    .HasConstraintName("fk_users_meetups_users_signups_guest_id"),
                joinEntity =>
                {
                    joinEntity.ToTable("meetups_users_signups");

                    joinEntity
                        .HasKey("meetup_id", "guest_id")
                        .HasName("pk_meetups_users_signups");

                    joinEntity
                        .HasIndex("guest_id")
                        .HasDatabaseName("ix_meetups_users_signups_guest_id");
                });
}
