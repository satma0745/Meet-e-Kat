namespace Meetekat.WebApi.Features.Auth.RegisterNewUser;

using System;
using System.Linq;
using BCrypt.Net;
using Meetekat.WebApi.Entities;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class RegisterNewUserFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public RegisterNewUserFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Auth)]
    [HttpPost("/api/users")]
    [SwaggerOperation("Register a new User.")]
    [SwaggerResponse(StatusCodes.Status201Created, "A new User was registered successfully.", typeof(RegisteredUserDto))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Specified username is already taken by some other user.")]
    public IActionResult RegisterNewUser([FromBody] RegisterUserDto registrationDto)
    {
        var usernameAlreadyTaken = context.Users.Any(user => user.Username == registrationDto.Username);
        if (usernameAlreadyTaken)
        {
            // TODO: Return the 400 Bad Request validation error, not 409 Conflict.
            return Conflict();
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = registrationDto.Username,
            Password = BCrypt.HashPassword(registrationDto.Password)
        };
        context.Users.Add(user);
        context.SaveChanges();

        var registeredDto = new RegisteredUserDto
        {
            Id = user.Id,
            Username = user.Username
        };
        return Created(registeredDto);
    }
}
