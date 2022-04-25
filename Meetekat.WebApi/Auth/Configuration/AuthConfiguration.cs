namespace Meetekat.WebApi.Auth.Configuration;

using System;
using System.Text;
using Meetekat.WebApi.Seedwork.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthConfiguration
{
    public static AuthConfiguration FromApplicationConfiguration(IConfiguration configuration)
    {
        var accessTokenLifetimeInMinutes = configuration.Select("Auth:AccessTokenLifetimeInMinutes").Required().AsInt();
        var accessTokenLifetime = TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);

        var refreshTokenLifetimeInDays = configuration.Select("Auth:RefreshTokenLifetimeInDays").Required().AsInt();
        var refreshTokenLifetime = TimeSpan.FromDays(refreshTokenLifetimeInDays);

        var secretKeyString = configuration.Select("Auth:SecretKey").Required().AsString();
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKeyString);
        var secretKey = new SymmetricSecurityKey(secretKeyBytes);
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretKey,
            
            ValidateAudience = false,
            ValidateIssuer = false,
            
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        return new AuthConfiguration
        {
            AccessTokenLifetime = accessTokenLifetime,
            RefreshTokenLifetime = refreshTokenLifetime,
            SigningCredentials = signingCredentials,
            TokenValidationParameters = tokenValidationParameters
        };
    }
    
    public TimeSpan AccessTokenLifetime { get; private init; }
    
    public TimeSpan RefreshTokenLifetime { get; private init; }
    
    public SigningCredentials SigningCredentials { get; private init; }
    
    public TokenValidationParameters TokenValidationParameters { get; private init; }
}
