namespace Meetekat.WebApi.Features.Auth.AuthenticateUser;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetekat.WebApi.Auth.Implementation;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

public class AuthenticateUserFeature : FeatureBase
{
    private readonly ApplicationContext context;
    private readonly JwtTokenService jwtTokenService;

    public AuthenticateUserFeature(ApplicationContext context, JwtTokenService jwtTokenService)
    {
        this.context = context;
        this.jwtTokenService = jwtTokenService;
    }

    [Tags(ApiSections.Auth)]
    [HttpPost("/api/auth/issue-token-pair")]
    [SwaggerOperation("Authenticate a User with provided credentials.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User authenticated successfully.", typeof(TokenPairDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User with the provided credentials doesn't exist.")]
    public async Task<IActionResult> AuthenticateUser([FromBody] SigningCredentialsDto credentialsDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Username == credentialsDto.Username);
        if (user is null || !BCrypt.Verify(credentialsDto.Password, user.Password))
        {
            // TODO: Return the 400 Bad Request validation error, not 404 Not Found.
            return NotFound();
        }

        var refreshToken = new RefreshToken
        {
            TokenId = Guid.NewGuid(),
            UserId = user.Id
        };
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();
        
        var tokenPair = jwtTokenService.IssueTokenPair(user, refreshToken.TokenId);
        var tokenPairDto = new TokenPairDto
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        };
        return Ok(tokenPairDto);
    }
}
