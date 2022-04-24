﻿namespace Meetekat.WebApi.Features.Auth.GetCurrentUserInfo;

using System.Linq;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class GetCurrentUserInfoFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public GetCurrentUserInfoFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Auth)]
    [HttpGet("/api/users/self")]
    [Authorize]
    [SwaggerOperation("Get current User info.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Current User info retrieved successfully.", typeof(CurrentUserDto))]
    public IActionResult GetCurrentUserInfo()
    {
        var currentUser = context.Users.SingleOrDefault(user => user.Id == CurrentUser.Id);
        if (currentUser is null)
        {
            // This can happen when User is deleted, but his token is still not expired.
            return Unauthorized();
        }

        var currentUserDto = new CurrentUserDto
        {
            Id = currentUser.Id,
            Username = currentUser.Username
        };
        return Ok(currentUserDto);
    }
}
