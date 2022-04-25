namespace Meetekat.WebApi.Features.Auth.RefreshTokenPair;

using System;
using System.Linq;
using Meetekat.WebApi.Auth.Implementation;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class RefreshTokenPairFeature : FeatureBase
{
    private readonly ApplicationContext context;
    private readonly JwtTokenService jwtTokenService;

    public RefreshTokenPairFeature(ApplicationContext context, JwtTokenService jwtTokenService)
    {
        this.context = context;
        this.jwtTokenService = jwtTokenService;
    }

    [Tags(ApiSections.Auth)]
    [HttpPost("/api/auth/refresh-token-pair")]
    [SwaggerOperation("Refresh Token Pair.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Token Pair was refreshed successfully.", typeof(TokenPairDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid, expired, used or fake Refresh Token was provided.")]
    public IActionResult RefreshTokenPair([FromBody] string refreshToken)
    {
        if (!jwtTokenService.TryParseRefreshToken(refreshToken, out var refreshTokenPayload))
        {
            return BadRequest();
        }

        var refreshTokenRecord = context.RefreshTokens.SingleOrDefault(token => token.TokenId == refreshTokenPayload.TokenId);
        if (refreshTokenRecord is null)
        {
            return BadRequest();
        }

        var user = context.Users.SingleOrDefault(user => user.Id == refreshTokenRecord.UserId);
        if (user is null)
        {
            return BadRequest();
        }
        
        var newRefreshTokenRecord = new RefreshToken
        {
            TokenId = Guid.NewGuid(),
            UserId = refreshTokenRecord.UserId
        };
        
        // Replace old Refresh Token with the newly created one (prevents multiple use of the Refresh Token).
        context.RefreshTokens.Add(newRefreshTokenRecord);
        context.RefreshTokens.Remove(refreshTokenRecord);
        
        context.SaveChanges();

        var tokenPair = jwtTokenService.IssueTokenPair(user, newRefreshTokenRecord.TokenId);
        var tokenPairDto = new TokenPairDto
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        };
        return Ok(tokenPairDto);
    }
}
