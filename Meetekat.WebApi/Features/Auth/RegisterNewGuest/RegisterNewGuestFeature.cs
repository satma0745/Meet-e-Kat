namespace Meetekat.WebApi.Features.Auth.RegisterNewGuest;

using System;
using System.Linq;
using BCrypt.Net;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class RegisterNewGuestFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public RegisterNewGuestFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Auth)]
    [HttpPost("/api/auth/register-new-guest")]
    [SwaggerOperation("Register a new Guest.")]
    [SwaggerResponse(StatusCodes.Status201Created, "A new Guest was registered successfully.", typeof(RegisteredGuestDto))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Specified username is already taken by some other user.")]
    public IActionResult RegisterNewGuest([FromBody] RegisterGuestDto registrationDto)
    {
        var usernameAlreadyTaken = context.Users.Any(user => user.Username == registrationDto.Username);
        if (usernameAlreadyTaken)
        {
            // TODO: Return the 400 Bad Request validation error, not 409 Conflict.
            return Conflict();
        }

        var guest = new Guest
        {
            Id = Guid.NewGuid(),
            Username = registrationDto.Username,
            Password = BCrypt.HashPassword(registrationDto.Password)
        };
        context.Guests.Add(guest);
        context.SaveChanges();

        var registeredDto = new RegisteredGuestDto
        {
            Id = guest.Id,
            Username = guest.Username,
            Role = guest.Role
        };
        return Created(registeredDto);
    }
}
