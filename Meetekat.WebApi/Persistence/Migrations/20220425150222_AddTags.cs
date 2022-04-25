using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetekat.WebApi.Persistence.Migrations
{
    public partial class AddTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CreateTagsTable(migrationBuilder);
            PopulateTagsTable(migrationBuilder);
            
            CreateJoinTable(migrationBuilder);
            PopulateJoinTable(migrationBuilder);

            DropTagsColumn(migrationBuilder);
        }

        private static void CreateTagsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_tags", x => x.id));
            
            migrationBuilder.CreateIndex(
                name: "ux_tags_name",
                table: "tags",
                column: "name",
                unique: true);
        }
        
        private static void PopulateTagsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO tags
                SELECT gen_random_uuid(), distinct_tag_names.name
                FROM (
                    SELECT DISTINCT unnest(string_to_array(tags, ';')) AS name
                    FROM meetups
                ) as distinct_tag_names;
            ");
        }

        private static void CreateJoinTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "meetups_tagging",
                columns: table => new
                {
                    meetup_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_meetups_tagging", x => new { x.meetup_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_meetups_meetups_tagging_meetup_id",
                        column: x => x.meetup_id,
                        principalTable: "meetups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tags_meetups_tagging_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_meetups_tagging_tag_id",
                table: "meetups_tagging",
                column: "tag_id");
        }

        private static void PopulateJoinTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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
            ");
        }
        
        private static void DropTagsColumn(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tags",
                table: "meetups");
        }
    }
}
