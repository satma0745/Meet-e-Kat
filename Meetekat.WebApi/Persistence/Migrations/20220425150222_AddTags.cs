namespace Meetekat.WebApi.Persistence.Migrations;

using System;
using Meetekat.WebApi.Seedwork.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(ApplicationContext))]
[Migration("20220425150222_AddTags")]
public class AddTags : SqlScriptMigration
{
    protected override string MigrationScript => @"
        CREATE TABLE tags (
            id uuid NOT NULL,
            name text NOT NULL,
            CONSTRAINT pk_tags PRIMARY KEY (id)
        );
        CREATE UNIQUE INDEX ux_tags_name ON tags (name);

        INSERT INTO tags
        SELECT
            gen_random_uuid(),
            distinct_tag_names.name
        FROM (
            SELECT DISTINCT unnest(string_to_array(tags, ';')) AS name
            FROM meetups
        ) as distinct_tag_names;


        CREATE TABLE meetups_tagging (
            meetup_id uuid NOT NULL,
            tag_id uuid NOT NULL,
            CONSTRAINT pk_meetups_tagging PRIMARY KEY (meetup_id, tag_id),
            CONSTRAINT fk_meetups_meetups_tagging_meetup_id FOREIGN KEY (meetup_id) REFERENCES meetups (id) ON DELETE CASCADE,
            CONSTRAINT fk_tags_meetups_tagging_tag_id FOREIGN KEY (tag_id) REFERENCES tags (id) ON DELETE CASCADE
        );
        CREATE INDEX ix_meetups_tagging_tag_id ON meetups_tagging (tag_id);

        INSERT INTO meetups_tagging
        SELECT
            tag_names_per_meetup.meetup_id,
            tags.id AS tag_id
        FROM (
            SELECT
                id AS meetup_id,
                unnest(string_to_array(tags, ';')) AS name
            FROM meetups
            GROUP BY meetup_id
        ) AS tag_names_per_meetup
        JOIN tags ON tags.name = tag_names_per_meetup.name;


        ALTER TABLE meetups DROP COLUMN tags;";
}
