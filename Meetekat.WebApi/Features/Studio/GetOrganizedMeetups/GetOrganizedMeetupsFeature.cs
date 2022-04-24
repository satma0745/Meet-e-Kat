namespace Meetekat.WebApi.Features.Studio.GetOrganizedMeetups;

using System.Collections.Generic;
using System.Linq;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

public class GetOrganizedMeetupsFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public GetOrganizedMeetupsFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Studio)]
    [HttpGet("/api/studio/get-organized-meetups")]
    [Authorize(Roles = nameof(Organizer))]
    [SwaggerOperation("Get all Meetups organized by the current User.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Meetups retrieved successfully.", typeof(IEnumerable<OrganizedMeetupDto>))]
    public IActionResult GetOrganizedMeetups()
    {
        var meetupOutputDtos = context.Meetups
            .Include(meetup => meetup.SignedUpGuests)
            .Where(meetup => meetup.OrganizerId == Caller.UserId)
            .Select(meetup => new Feed.GetMeetups.MeetupDto
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
