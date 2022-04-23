namespace Meetekat.WebApi.Features.Meetups.RegisterNewMeetup;

using System;
using System.Net.Mime;
using Meetekat.WebApi.Entities;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Tags("Meetups")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class RegisterNewMeetupFeature : ControllerBase
{
    private readonly ApplicationContext context;

    public RegisterNewMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [HttpPost("/api/meetups")]
    [SwaggerOperation("Register a new meetup.")]
    [SwaggerResponse(StatusCodes.Status201Created, "A new meetup is registered successfully.", typeof(RegisteredMeetupDto))]
    public IActionResult RegisterNewMeetup([FromBody] RegisterMeetupDto registerDto)
    {
        var meetup = new Meetup
        {
            Id = Guid.NewGuid(),
            Title = registerDto.Title,
            Description = registerDto.Description,
            Tags = registerDto.Tags,
            StartTime = registerDto.StartTime,
            EndTime = registerDto.EndTime,
            Organizer = registerDto.Organizer
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
            Organizer = meetup.Organizer
        };
        return StatusCode(StatusCodes.Status201Created, registeredDto);
    }
}
