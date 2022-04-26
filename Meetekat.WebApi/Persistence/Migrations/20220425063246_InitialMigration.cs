namespace Meetekat.WebApi.Persistence.Migrations;

using Meetekat.WebApi.Seedwork.Persistence;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

[DbContext(typeof(ApplicationContext))]
[Migration("20220425063246_InitialMigration")]
public class InitialMigration : SqlScriptMigration
{
    // This Migrations exists only to set up a Migration History.
    protected override string MigrationScript => string.Empty;
}
