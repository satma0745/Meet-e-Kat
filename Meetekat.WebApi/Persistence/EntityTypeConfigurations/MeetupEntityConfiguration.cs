namespace Meetekat.WebApi.Persistence.EntityTypeConfigurations;

using System;
using Meetekat.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MeetupEntityConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> meetupEntity)
    {
        meetupEntity.ToTable("meetups");

        meetupEntity
            .HasKey(meetup => meetup.Id)
            .HasName("pk_meetups");

        meetupEntity
            .HasIndex(meetup => meetup.OrganizerId)
            .HasDatabaseName("ix_meetups_organizer_id");

        meetupEntity
            .Property(meetup => meetup.Id)
            .HasColumnName("id");

        meetupEntity
            .Property(meetup => meetup.Title)
            .IsRequired()
            .HasColumnName("title");

        meetupEntity
            .Property(meetup => meetup.Description)
            .IsRequired()
            .HasColumnName("description");
        
        meetupEntity
            .Property(meetup => meetup.Tags)
            .IsRequired()
            .HasConversion(
                tags => string.Join(';', tags),
                aggregate => aggregate.Split(';', StringSplitOptions.None))
            .HasColumnName("tags");

        meetupEntity
            .Property(meetup => meetup.StartTime)
            .HasColumnName("start_time");

        meetupEntity
            .Property(meetup => meetup.EndTime)
            .HasColumnName("end_time");

        meetupEntity
            .Property(meetup => meetup.OrganizerId)
            .HasColumnName("organizer_id");
    }
}
