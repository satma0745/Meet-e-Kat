namespace Meetekat.WebApi.Swagger;

using Meetekat.WebApi.Seedwork.Configuration;
using Microsoft.Extensions.Configuration;

public class SwaggerConfiguration
{
    public static SwaggerConfiguration FromApplicationConfiguration(IConfiguration configuration) =>
        new()
        {
            SwaggerEnabled = configuration.Select("Swagger:SwaggerEnabled").Required().AsBool(),
            UiRoutePrefix = configuration.Select("Swagger:UiRoutePrefix").Required().AsString(),
            HideSchemasSection = configuration.Select("Swagger:HideSchemasSection").Required().AsBool()
        };
    
    public bool SwaggerEnabled { get; private init; }
    
    public string UiRoutePrefix { get; private init; }
    
    public bool HideSchemasSection { get; private init; }
}
