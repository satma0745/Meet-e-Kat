namespace Meetekat.WebApi.Features.Auth.RegisterNewOrganizer;

using System;
using System.Linq;
using BCrypt.Net;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class RegisterNewOrganizerFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public RegisterNewOrganizerFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Auth)]
    [HttpPost("/api/auth/register-new-organizer")]
    [SwaggerOperation("Register a new Organizer.")]
    [SwaggerResponse(StatusCodes.Status201Created, "A new Organizer was registered successfully.", typeof(RegisteredOrganizerDto))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Specified username is already taken by some other user.")]
    public IActionResult RegisterNewOrganizer([FromBody] RegisterOrganizerDto registrationDto)
    {
        var usernameAlreadyTaken = context.Users.Any(user => user.Username == registrationDto.Username);
        if (usernameAlreadyTaken)
        {
            // TODO: Return the 400 Bad Request validation error, not 409 Conflict.
            return Conflict();
        }

        var guest = new Organizer
        {
            Id = Guid.NewGuid(),
            Username = registrationDto.Username,
            Password = BCrypt.HashPassword(registrationDto.Password)
        };
        context.Organizers.Add(guest);
        context.SaveChanges();

        var registeredDto = new RegisteredOrganizerDto
        {
            Id = guest.Id,
            Username = guest.Username,
            Role = guest.Role
        };
        return Created(registeredDto);
    }
}
