namespace Meetekat.WebApi.Swagger;

using Microsoft.AspNetCore.Builder;

public static class SwaggerMiddlewareConfigurationExtensions
{
    public static IApplicationBuilder UseSwaggerDocs(this WebApplication webApplication)
    {
        var swaggerConfiguration = SwaggerConfiguration.FromApplicationConfiguration(webApplication.Configuration);

        if (swaggerConfiguration.SwaggerEnabled)
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI(options =>
            {
                if (swaggerConfiguration.HideSchemasSection)
                {
                    options.DefaultModelsExpandDepth(-1);
                }

                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = swaggerConfiguration.UiRoutePrefix;
            });
        }

        return webApplication;
    }
}
