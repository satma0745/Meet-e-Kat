namespace Meetekat.WebApi.Auth;

using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>Enriches Swagger Documentation with Security Requirements info (padlock icons in the Swagger UI).</summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.Itself)]
public class OpenApiSecurityRequirementFilter: IOperationFilter
{
    private static readonly OpenApiSecurityRequirement AuthenticationRequirement = new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        
        // Require authentication if action is marked with the [Authorize] attribute
        var markedWithAuthorize = actionMetadata.Any(metadataItem => metadataItem is AuthorizeAttribute);
        if (markedWithAuthorize)
        {
            AddSecurityRequirement(operation);
        }
    }

    private static void AddSecurityRequirement(OpenApiOperation operation)
    {
        operation.Security.Add(AuthenticationRequirement);
            
        if (!operation.Responses.ContainsKey("401"))
        {
            var unauthorizedResponse = new OpenApiResponse {Description = "User must be authorized to perform this action."};
            operation.Responses.Add("401", unauthorizedResponse);
        }
    }
}
