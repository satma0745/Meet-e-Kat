namespace Meetekat.WebApi.Features.Meetups.GetMeetups;

using System.Collections.Generic;
using System.Linq;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

public class GetMeetupsFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public GetMeetupsFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Meetups)]
    [HttpGet("/api/meetups")]
    [SwaggerOperation("Get all meetups.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Meetups retrieved successfully.", typeof(IEnumerable<MeetupDto>))]
    public IActionResult GetMeetups()
    {
        var meetupOutputDtos = context.Meetups
            .Include(meetup => meetup.SignedUpGuests)
            .Select(meetup => new MeetupDto
            {
                Id = meetup.Id,
                Title = meetup.Title,
                Description = meetup.Description,
                Tags = meetup.Tags,
                StartTime = meetup.StartTime,
                EndTime = meetup.EndTime,
                OrganizerId = meetup.OrganizerId,
                SignedUpGuestsCount = meetup.SignedUpGuests.Count
            });

        return Ok(meetupOutputDtos);
    }
}
