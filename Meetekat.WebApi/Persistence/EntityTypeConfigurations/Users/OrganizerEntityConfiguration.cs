namespace Meetekat.WebApi.Persistence.EntityTypeConfigurations.Users;

using Meetekat.WebApi.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrganizerEntityConfiguration : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> organizerEntity) =>
        organizerEntity
            .HasMany(organizer => organizer.OrganizedMeetups)
            .WithOne(meetup => meetup.Organizer)
            .HasForeignKey(meetup => meetup.OrganizerId)
            .HasConstraintName("fk_organizers_meetups_organizer_id");
}
