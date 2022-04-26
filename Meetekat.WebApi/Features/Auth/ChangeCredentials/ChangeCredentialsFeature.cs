namespace Meetekat.WebApi.Features.Auth.ChangeCredentials;

using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
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
    [HttpPost("/api/auth/change-credentials")]
    [Authorize]
    [SwaggerOperation("Change signing credentials for a User with the specified ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Signing credentials were updated successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User with the specified ID doesn't exist.")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Specified username is already taken by some other user.")]
    public async Task<IActionResult> ChangeCredentials([FromBody] ChangeCredentialsDto credentialsDto)
    {
        var user = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Id == Caller.UserId);
        if (user is null)
        {
            return NotFound();
        }

        // In this query we're excluding target User. This way User can preserve the Username
        // and only change the Password (without getting Username uniqueness violation error). 
        var usernameAlreadyTaken = await context.Users
            .Where(anotherUser => anotherUser.Id != user.Id)
            .AnyAsync(anotherUser => anotherUser.Username == credentialsDto.Username);
        if (usernameAlreadyTaken)
        {
            // TODO: Return the 400 Bad Request validation error, not 409 Conflict.
            return Conflict();
        }

        user.Username = credentialsDto.Username;
        user.Password = BCrypt.HashPassword(credentialsDto.Password);
        
        // Revoke refresh tokens.
        user.RefreshTokens.Clear();
        
        await context.SaveChangesAsync();

        return Ok();
    }
}
