namespace Meetekat.WebApi.Features.Auth.ChangeCredentials;

using System.Linq;
using BCrypt.Net;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

public class ChangeCredentialsFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public ChangeCredentialsFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Auth)]
    [HttpPut("/api/users/self/credentials")]
    [Authorize]
    [SwaggerOperation("Change signing credentials for a User with the specified ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Signing credentials were updated successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User with the specified ID doesn't exist.")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Specified username is already taken by some other user.")]
    public IActionResult ChangeCredentials([FromBody] ChangeCredentialsDto credentialsDto)
    {
        var user = context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefault(user => user.Id == CurrentUser.Id);
        if (user is null)
        {
            return NotFound();
        }

        // In this query we're excluding target User. This way User can preserve the Username
        // and only change the Password (without getting Username uniqueness violation error). 
        var usernameAlreadyTaken = context.Users
            .Where(anotherUser => anotherUser.Id != user.Id)
            .Any(anotherUser => anotherUser.Username == credentialsDto.Username);
        if (usernameAlreadyTaken)
        {
            // TODO: Return the 400 Bad Request validation error, not 409 Conflict.
            return Conflict();
        }

        user.Username = credentialsDto.Username;
        user.Password = BCrypt.HashPassword(credentialsDto.Password);
        
        // Revoke refresh tokens.
        user.RefreshTokens.Clear();
        
        context.SaveChanges();

        return Ok();
    }
}
