using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetekat.WebApi.Persistence.Migrations
{
    public partial class AddMeetupsUsersRefreshTokensGuestsMeetups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "meetups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    tags = table.Column<string>(type: "text", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    organizer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_meetups", x => x.id);
                    table.ForeignKey(
                        name: "fk_organizers_meetups_organizer_id",
                        column: x => x.organizer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    token_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.token_id);
                    table.ForeignKey(
                        name: "fk_users_refresh_tokens_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meetups_users_signups",
                columns: table => new
                {
                    meetup_id = table.Column<Guid>(type: "uuid", nullable: false),
                    guest_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_meetups_users_signups", x => new { x.meetup_id, x.guest_id });
                    table.ForeignKey(
                        name: "fk_meetups_meetups_users_signups_meetup_id",
                        column: x => x.meetup_id,
                        principalTable: "meetups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_meetups_users_signups_guest_id",
                        column: x => x.guest_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_meetups_organizer_id",
                table: "meetups",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "ix_meetups_users_signups_guest_id",
                table: "meetups_users_signups",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ux_users_username",
                table: "users",
                column: "username",
                unique: true);
        }
    }
}
