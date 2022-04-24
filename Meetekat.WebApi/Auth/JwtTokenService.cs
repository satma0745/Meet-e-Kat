namespace Meetekat.WebApi.Auth;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService
{
    private readonly TimeSpan accessTokenLifetime;
    private readonly TimeSpan refreshTokenLifetime;
    private readonly SigningCredentials signingCredentials;
    private readonly JwtSecurityTokenHandler tokenHandler;
    private readonly TokenValidationParameters tokenValidationParameters;

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
        
        const string refreshTokenLifetimeInDaysParameterName = "Auth:RefreshTokenLifetimeInDays";
        var refreshTokenLifetimeInDaysParameter = configuration[refreshTokenLifetimeInDaysParameterName];
        if (string.IsNullOrWhiteSpace(refreshTokenLifetimeInDaysParameter))
        {
            throw new Exception($"Configuration parameter \"{refreshTokenLifetimeInDaysParameterName}\" is required.");
        }
        if (!int.TryParse(refreshTokenLifetimeInDaysParameter, out var refreshTokenLifetimeInDays))
        {
            throw new Exception($"Configuration parameter \"{refreshTokenLifetimeInDaysParameterName}\" must be a number.");
        }
        refreshTokenLifetime = TimeSpan.FromDays(refreshTokenLifetimeInDays);

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
        
        tokenValidationParameters = new TokenValidationParameters
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
    }

    public bool TryParseRefreshToken(string refreshToken, out RefreshTokenPayload refreshTokenPayload)
    {
        try
        {
            var claims = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out _).Claims;

            var tokenIdClaim = claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier);
            var tokenId = Guid.Parse(tokenIdClaim.Value);

            refreshTokenPayload = new RefreshTokenPayload
            {
                TokenId = tokenId
            };
            return true;
        }
        catch (Exception)
        {
            refreshTokenPayload = default;
            return false;
        }
    }

    public TokenPair IssueTokenPair(Guid userId, Guid refreshTokenId)
    {
        var accessTokenClaims = new Dictionary<string, object>
        {
            {ClaimTypes.NameIdentifier, userId}
        };
        var accessToken = IssueToken(accessTokenClaims, accessTokenLifetime);

        var refreshTokenClaims = new Dictionary<string, object>
        {
            {ClaimTypes.NameIdentifier, refreshTokenId}
        };
        var refreshToken = IssueToken(refreshTokenClaims, refreshTokenLifetime);

        return new TokenPair
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string IssueToken(IDictionary<string, object> claims, TimeSpan lifetime)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = signingCredentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
