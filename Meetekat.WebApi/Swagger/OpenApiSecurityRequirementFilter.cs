namespace Meetekat.WebApi.Swagger;

using System;
using System.Collections.Generic;
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
        
        // Only add authentication requirements if action is marked with the [Authorize] attribute.
        // Action might also have multiple [Authorize] attributes.
        var authorizeAttributes = actionMetadata.OfType<AuthorizeAttribute>().ToList();
        if (!authorizeAttributes.Any())
        {
            return;
        }
        
        operation.Security.Add(AuthenticationRequirement);
        AddFallback401UnauthorizedResponse(operation);
        AddFallback403ForbiddenResponse(operation, authorizeAttributes);
    }

    private static void AddFallback401UnauthorizedResponse(OpenApiOperation operation)
    {
        const string statusCode401Unauthorized = "401";
        
        if (operation.Responses.ContainsKey(statusCode401Unauthorized))
        {
            // In some cases we might want to specify custom 401 Unauthorized response.
            return;
        }

        const string message = "User must be authorized to perform this action.";
        var unauthorizedResponse = new OpenApiResponse {Description = message};
        operation.Responses.Add(statusCode401Unauthorized, unauthorizedResponse);
    }
    
    private static void AddFallback403ForbiddenResponse(OpenApiOperation operation, IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        const string statusCode403Forbidden = "403";

        // The "Roles" field is a comma delimited list of roles.
        var roles = authorizeAttributes
            .Select(authorizeAttribute => authorizeAttribute.Roles)
            .Where(roles => !string.IsNullOrWhiteSpace(roles))
            .SelectMany(roles => roles.Split(","))
            .ToList();
        if (!roles.Any())
        {
            // Any user can perform this action.
            return;
        }
        
        if (operation.Responses.ContainsKey(statusCode403Forbidden))
        {
            // In some cases we might want to specify custom 403 Forbidden response.
            return;
        }

        var message = roles.Count switch
        {
            1 => $"Only a User with the \"{roles.Single()}\" Role can perform this action.",
            _ => $"Only a User with one of the [{string.Join(", ", roles.Select(role => $"\"{role}\""))}] Roles can perform this action."
        };
        var forbiddenResponse = new OpenApiResponse {Description = message};
        operation.Responses.Add(statusCode403Forbidden, forbiddenResponse);
    }
}
