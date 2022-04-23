namespace Meetekat.WebApi.Auth;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService
{
    private readonly TimeSpan accessTokenLifetime;
    private readonly SigningCredentials signingCredentials;
    private readonly JwtSecurityTokenHandler tokenHandler;

    public JwtTokenService(IConfiguration configuration)
    {
        // TODO: Refactor Auth configuration.
        
        const string accessTokenLifetimeInMinutesParameterName = "Auth:AccessTokenLifetimeInMinutes";
        var accessTokenLifetimeInMinutesParameter = configuration[accessTokenLifetimeInMinutesParameterName];
        if (string.IsNullOrWhiteSpace(accessTokenLifetimeInMinutesParameter))
        {
            throw new Exception($"Configuration parameter \"{accessTokenLifetimeInMinutesParameterName}\" is required.");
        }
        if (!int.TryParse(accessTokenLifetimeInMinutesParameter, out var accessTokenLifetimeInMinutes))
        {
            throw new Exception($"Configuration parameter \"{accessTokenLifetimeInMinutesParameterName}\" must be a number.");
        }
        accessTokenLifetime = TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);

        const string secretKeyParameterName = "Auth:SecretKey";
        var secretKeyParameter = configuration[secretKeyParameterName];
        if (string.IsNullOrWhiteSpace(secretKeyParameter))
        {
            throw new Exception($"Configuration parameter \"{secretKeyParameterName}\" is required.");
        }
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKeyParameter);
        var secretKey = new SymmetricSecurityKey(secretKeyBytes);
        signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

        tokenHandler = new JwtSecurityTokenHandler();
    }

    public string IssueAccessToken(Guid userId)
    {
        var claims = new Dictionary<string, object>
        {
            {ClaimTypes.NameIdentifier, userId}
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            Expires = DateTime.UtcNow.Add(accessTokenLifetime),
            SigningCredentials = signingCredentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
