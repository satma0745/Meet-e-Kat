namespace Meetekat.WebApi.Features;

using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class FeatureBase : ControllerBase
{
    /// <summary>Contains current User's info.</summary>
    /// <returns>A <see cref="CurrentUserInfo"/> instance containing User's info.</returns>
    protected CurrentUserInfo CurrentUser => lazyCurrentUser.Value;

    private readonly Lazy<CurrentUserInfo> lazyCurrentUser;

    protected FeatureBase() =>
        lazyCurrentUser = new Lazy<CurrentUserInfo>(GetCurrentUserInfo);
    
    /// <summary>
    /// Creates an <see cref="ObjectResult"/> object
    /// that produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
    protected ObjectResult Created(object value) =>
        StatusCode(StatusCodes.Status201Created, value);

    private CurrentUserInfo GetCurrentUserInfo()
    {
        try
        {
            var currentUserIdClaim = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier);
            var currentUserId = Guid.Parse(currentUserIdClaim.Value);

            return new CurrentUserInfo
            {
                Id = currentUserId
            };
        }
        catch (Exception exception)
        {
            throw new Exception("Api caller isn't authenticated.", exception);
        }
    }
}

public class CurrentUserInfo
{
    public Guid Id { get; init; }
}
