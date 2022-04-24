namespace Meetekat.WebApi.Features.Studio.RegisterNewMeetup;

using System;
using System.Linq;
using Meetekat.WebApi.Entities;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class RegisterNewMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public RegisterNewMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Studio)]
    [HttpPost("/api/meetups")]
    [Authorize(Roles = nameof(Organizer))]
    [SwaggerOperation("Register a new meetup.")]
    [SwaggerResponse(StatusCodes.Status201Created, "A new meetup is registered successfully.", typeof(RegisteredMeetupDto))]
    public IActionResult RegisterNewMeetup([FromBody] RegisterMeetupDto registerDto)
    {
        var organizerExists = context.Organizers.Any(organizer => organizer.Id == Caller.UserId);
        if (!organizerExists)
        {
            // Can happen if deleted Organizer tries to register a new Meetup (if the Access Token hasn't yet expired).
            return Unauthorized();
        }
        
        var meetup = new Meetup
        {
            Id = Guid.NewGuid(),
            Title = registerDto.Title,
            Description = registerDto.Description,
            Tags = registerDto.Tags,
            StartTime = registerDto.StartTime,
            EndTime = registerDto.EndTime,
            OrganizerId = Caller.UserId
        };

        context.Meetups.Add(meetup);
        context.SaveChanges();

        var registeredDto = new RegisteredMeetupDto
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Tags = meetup.Tags,
            StartTime = meetup.StartTime,
            EndTime = meetup.EndTime,
            OrganizerId = meetup.OrganizerId
        };
        return Created(registeredDto);
    }
}
