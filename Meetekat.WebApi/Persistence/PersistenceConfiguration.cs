namespace Meetekat.WebApi.Persistence;

using Meetekat.WebApi.Seedwork.Configuration;
using Microsoft.Extensions.Configuration;

public class PersistenceConfiguration
{
    public static PersistenceConfiguration FromApplicationConfiguration(IConfiguration applicationConfiguration)
    {
        var host = applicationConfiguration.Select("Persistence:Host").Required().AsString();
        var port = applicationConfiguration.Select("Persistence:Port").Required().AsInt();
        var database = applicationConfiguration.Select("Persistence:Database").Required().AsString();
        var username = applicationConfiguration.Select("Persistence:Username").Required().AsString();
        var password = applicationConfiguration.Select("Persistence:Password").Required().AsString();

        var connectionString = $"Server={host};Port={port};Database={database};User Id={username};Password={password}";
        return new PersistenceConfiguration {ConnectionString = connectionString};
    }
    
    public string ConnectionString { get; private init; }
}
