namespace Meetekat.WebApi.Features.Feed.GetMeetups;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    [Tags(ApiSections.Feed)]
    [HttpGet("/api/feed/get-meetups")]
    [SwaggerOperation("Get all meetups.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Meetups retrieved successfully.", typeof(IEnumerable<MeetupDto>))]
    public async Task<IActionResult> GetMeetups()
    {
        var meetupOutputDtos = await context.Meetups
            .Include(meetup => meetup.Tags)
            .Include(meetup => meetup.SignedUpGuests)
            .Select(meetup => new MeetupDto
            {
                Id = meetup.Id,
                Title = meetup.Title,
                Description = meetup.Description,
                Tags = meetup.Tags.Select(tag => tag.Name),
                StartTime = meetup.StartTime,
                EndTime = meetup.EndTime,
                OrganizerId = meetup.OrganizerId,
                SignedUpGuestsCount = meetup.SignedUpGuests.Count
            })
            .ToListAsync();

        return Ok(meetupOutputDtos);
    }
}
