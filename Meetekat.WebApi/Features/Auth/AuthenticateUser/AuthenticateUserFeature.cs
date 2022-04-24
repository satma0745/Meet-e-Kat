namespace Meetekat.WebApi.Features.Auth.AuthenticateUser;

using System;
using System.Linq;
using BCrypt.Net;
using Meetekat.WebApi.Auth;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [HttpPost("/api/token-pairs")]
    [SwaggerOperation("Authenticate a User with provided credentials.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User authenticated successfully.", typeof(TokenPairDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User with the provided credentials doesn't exist.")]
    public IActionResult AuthenticateUser([FromBody] SigningCredentialsDto credentialsDto)
    {
        var user = context.Users.SingleOrDefault(user => user.Username == credentialsDto.Username);
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
        context.SaveChanges();
        
        var tokenPair = jwtTokenService.IssueTokenPair(user, refreshToken.TokenId);
        var tokenPairDto = new TokenPairDto
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        };
        return Ok(tokenPairDto);
    }
}
