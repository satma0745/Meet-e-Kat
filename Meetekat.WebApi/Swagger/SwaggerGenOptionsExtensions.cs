namespace Meetekat.WebApi.Swagger;

using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerGenOptionsExtensions
{
    public static void UseTypeFullNameForDtoSchemas(this SwaggerGenOptions options) =>
        options.CustomSchemaIds(dtoType => dtoType.FullName);

    public static void IncludeXmlComments(this SwaggerGenOptions options)
    {
        var webApiProjectName = Assembly.GetExecutingAssembly().GetName().Name;
        var pathToXmlCommentsFile = Path.Combine(AppContext.BaseDirectory, $"{webApiProjectName}.xml");
        options.IncludeXmlComments(pathToXmlCommentsFile);
    }
    
    public static void IncludeSwaggerAnnotations(this SwaggerGenOptions options) =>
        options.OperationFilter<AnnotationsOperationFilter>();

    public static void AddJwtSecurity(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Put Your access token here:",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        
        options.OperationFilter<OpenApiSecurityRequirementFilter>();
    }
}
