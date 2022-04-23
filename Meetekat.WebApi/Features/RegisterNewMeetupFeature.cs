namespace Meetekat.WebApi.Features;

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
    [SwaggerResponse(StatusCodes.Status201Created, "A new meetup is registered successfully.", typeof(Meetup))]
    public IActionResult RegisterNewMeetup([FromBody] Meetup meetup)
    {
        meetup.Id = Guid.NewGuid();
        
        context.Meetups.Add(meetup);
        context.SaveChanges();
        
        return StatusCode(StatusCodes.Status201Created, meetup);
    }
}
