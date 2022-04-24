namespace Meetekat.WebApi.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authConfiguration = AuthConfiguration.FromApplicationConfiguration(configuration);
        
        services
            .AddSingleton(() => new JwtTokenService(authConfiguration))
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = authConfiguration.TokenValidationParameters;
                options.RequireHttpsMetadata = false;
            });
        
        return services;
    }
}
