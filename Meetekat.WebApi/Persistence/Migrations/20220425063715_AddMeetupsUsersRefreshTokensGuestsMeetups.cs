namespace Meetekat.WebApi.Persistence.Migrations;

using System;
using Meetekat.WebApi.Seedwork.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(ApplicationContext))]
[Migration("20220425063715_AddMeetupsUsersRefreshTokensGuestsMeetups")]
public class AddMeetupsUsersRefreshTokensGuestsMeetups : SqlScriptMigration
{
    protected override string MigrationScript => @"
        CREATE TABLE users (
            id uuid NOT NULL,
            username text NOT NULL,
            password text NOT NULL,
            role text NOT NULL,
            CONSTRAINT pk_users PRIMARY KEY (id)
        );
        CREATE UNIQUE INDEX ux_users_username ON users (username);

        CREATE TABLE refresh_tokens (
            token_id uuid NOT NULL,
            user_id uuid NOT NULL,
            CONSTRAINT pk_refresh_tokens PRIMARY KEY (token_id),
            CONSTRAINT fk_users_refresh_tokens_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
        );
        CREATE INDEX ix_refresh_tokens_user_id ON refresh_tokens (user_id);

        CREATE TABLE meetups (
            id uuid NOT NULL,
            title text NOT NULL,
            description text NOT NULL,
            tags text NOT NULL,
            start_time timestamp with time zone NOT NULL,
            end_time timestamp with time zone NOT NULL,
            organizer_id uuid NOT NULL,
            CONSTRAINT pk_meetups PRIMARY KEY (id),
            CONSTRAINT fk_organizers_meetups_organizer_id FOREIGN KEY (organizer_id) REFERENCES users (id) ON DELETE CASCADE
        );
        CREATE INDEX ix_meetups_organizer_id ON meetups (organizer_id);

        CREATE TABLE meetups_users_signups (
            meetup_id uuid NOT NULL,
            guest_id uuid NOT NULL,
            CONSTRAINT pk_meetups_users_signups PRIMARY KEY (meetup_id, guest_id),
            CONSTRAINT fk_meetups_meetups_users_signups_meetup_id FOREIGN KEY (meetup_id) REFERENCES meetups (id) ON DELETE CASCADE,
            CONSTRAINT fk_users_meetups_users_signups_guest_id FOREIGN KEY (guest_id) REFERENCES users (id) ON DELETE CASCADE
        );
        CREATE INDEX ix_meetups_users_signups_guest_id ON meetups_users_signups (guest_id);";
}
