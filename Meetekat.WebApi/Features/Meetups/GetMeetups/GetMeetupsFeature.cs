namespace Meetekat.WebApi.Features.Meetups.GetMeetups;

using System.Collections.Generic;
using System.Linq;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class GetMeetupsFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public GetMeetupsFeature(ApplicationContext context) =>
        this.context = context;

    [Tags("Meetups")]
    [HttpGet("/api/meetups")]
    [SwaggerOperation("Get all meetups.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Meetups retrieved successfully.", typeof(IEnumerable<MeetupDto>))]
    public IActionResult GetMeetups()
    {
        var meetupOutputDtos = context.Meetups.Select(meetup => new MeetupDto
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Tags = meetup.Tags,
            StartTime = meetup.StartTime,
            EndTime = meetup.EndTime,
            Organizer = meetup.Organizer
        });

        return Ok(meetupOutputDtos);
    }
}
