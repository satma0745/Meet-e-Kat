namespace Meetekat.WebApi.Features.Feed.GetMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

public class GetMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public GetMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Feed)]
    [HttpGet("/api/feed/get-meetup")]
    [SwaggerOperation("Get a specific Meetups with matching ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Meetup retrieved successfully.", typeof(MeetupDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Meetup with the specified ID doesn't exist")]
    public IActionResult GetMeetup([FromQuery] [Required] Guid meetupId)
    {
        var meetup = context.Meetups
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefault(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }
        
        var meetupOutputDto = new MeetupDto
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Tags = meetup.Tags,
            StartTime = meetup.StartTime,
            EndTime = meetup.EndTime,
            OrganizerId = meetup.OrganizerId,
            SignedUpGuestsCount = meetup.SignedUpGuests.Count
        };
        return Ok(meetupOutputDto);
    }
}
