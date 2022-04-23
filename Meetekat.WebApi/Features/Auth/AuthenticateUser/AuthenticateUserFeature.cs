namespace Meetekat.WebApi.Features.Auth.AuthenticateUser;

using System.Linq;
using BCrypt.Net;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class AuthenticateUserFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public AuthenticateUserFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Auth)]
    [HttpPost("/api/session")]
    [SwaggerOperation("Authenticate a User with provided credentials.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User authenticated successfully.", typeof(string))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User with the provided credentials doesn't exist.")]
    public IActionResult AuthenticateUser([FromBody] SigningCredentialsDto credentialsDto)
    {
        var user = context.Users.SingleOrDefault(user => user.Username == credentialsDto.Username);
        if (user is null || !BCrypt.Verify(credentialsDto.Password, user.Password))
        {
            // TODO: Return the 400 Bad Request validation error, not 404 Not Found.
            return NotFound();
        }

        var accessToken = user.Id.GetHashCode();
        return Ok(accessToken);
    }
}
