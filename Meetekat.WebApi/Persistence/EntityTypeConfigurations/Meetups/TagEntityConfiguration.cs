namespace Meetekat.WebApi.Persistence.EntityTypeConfigurations.Meetups;

using System.Collections.Generic;
using Meetekat.WebApi.Entities.Meetups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TagEntityConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> tagEntity)
    {
        tagEntity.ToTable("tags");

        tagEntity
            .HasKey(tag => tag.Id)
            .HasName("pk_tags");

        tagEntity
            .HasIndex(tag => tag.Name)
            .IsUnique()
            .HasDatabaseName("ux_tags_name");

        tagEntity
            .Property(tag => tag.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        tagEntity
            .Property(tag => tag.Name)
            .IsRequired()
            .HasColumnName("name");

        tagEntity
            .HasMany(tag => tag.TaggedMeetups)
            .WithMany(meetup => meetup.Tags)
            .UsingEntity<Dictionary<string, object>>(
                joinEntity => joinEntity
                    .HasOne<Meetup>()
                    .WithMany()
                    .HasForeignKey("meetup_id")
                    .HasConstraintName("fk_meetups_meetups_tagging_meetup_id"),
                joinEntity => joinEntity
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("tag_id")
                    .HasConstraintName("fk_tags_meetups_tagging_tag_id"),
                joinEntity =>
                {
                    joinEntity.ToTable("meetups_tagging");

                    joinEntity
                        .HasKey("meetup_id", "tag_id")
                        .HasName("pk_meetups_tagging");

                    joinEntity
                        .HasIndex("tag_id")
                        .HasDatabaseName("ix_meetups_tagging_tag_id");
                });
    }
}
