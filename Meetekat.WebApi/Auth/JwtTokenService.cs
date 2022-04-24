namespace Meetekat.WebApi.Auth;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Meetekat.WebApi.Entities.Users;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService
{
    private readonly AuthConfiguration configuration;
    private readonly JwtSecurityTokenHandler tokenHandler;

    public JwtTokenService(AuthConfiguration configuration)
    {
        this.configuration = configuration;
        tokenHandler = new JwtSecurityTokenHandler();
    }

    public bool TryParseRefreshToken(string refreshToken, out RefreshTokenPayload refreshTokenPayload)
    {
        try
        {
            var claims = tokenHandler.ValidateToken(refreshToken, configuration.TokenValidationParameters, out _).Claims;

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

    public TokenPair IssueTokenPair(User user, Guid refreshTokenId)
    {
        var accessTokenClaims = new Dictionary<string, object>
        {
            {ClaimTypes.NameIdentifier, user.Id},
            {ClaimTypes.Role, user.Role}
        };
        var accessToken = IssueToken(accessTokenClaims, configuration.AccessTokenLifetime);

        var refreshTokenClaims = new Dictionary<string, object>
        {
            {ClaimTypes.NameIdentifier, refreshTokenId}
        };
        var refreshToken = IssueToken(refreshTokenClaims, configuration.RefreshTokenLifetime);

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
            SigningCredentials = configuration.SigningCredentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
