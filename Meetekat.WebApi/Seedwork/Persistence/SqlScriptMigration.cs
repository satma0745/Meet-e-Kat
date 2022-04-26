namespace Meetekat.WebApi.Seedwork.Persistence;

using Microsoft.EntityFrameworkCore.Migrations;

public abstract class SqlScriptMigration : Migration
{
    protected abstract string MigrationScript { get; }

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        if (!string.IsNullOrWhiteSpace(MigrationScript))
        {
            migrationBuilder.Sql(MigrationScript);
        }
    }
}
