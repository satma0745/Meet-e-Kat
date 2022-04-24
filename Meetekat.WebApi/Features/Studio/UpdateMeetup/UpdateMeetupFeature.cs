namespace Meetekat.WebApi.Features.Studio.UpdateMeetup;

using System;
using System.Linq;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class UpdateMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public UpdateMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Studio)]
    [HttpPut("/api/meetups/{meetupId:guid}")]
    [Authorize(Roles = nameof(Organizer))]
    [SwaggerOperation("Update a Meetup with the matching ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "A Meetup with the specified ID was updated successfully.", typeof(UpdatedMeetupDto))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Only a Meetup's direct Organizer can update the Meetup.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "A Meetup with the specified ID doesn't exist.")]
    public IActionResult UpdateMeetup([FromRoute] Guid meetupId, [FromBody] UpdateMeetupDto updateDto)
    {
        var meetup = context.Meetups.SingleOrDefault(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }
        
        var organizerExists = context.Organizers.Any(organizer => organizer.Id == Caller.UserId);
        if (!organizerExists)
        {
            // Can happen if deleted Organizer tries to update a Meetup (if the Access Token hasn't yet expired).
            return Unauthorized();
        }
        if (meetup.OrganizerId != Caller.UserId)
        {
            // Only a Meetup's direct Organizer can update the Meetup.
            return Forbidden();
        }

        meetup.Title = updateDto.Title;
        meetup.Description = updateDto.Description;
        meetup.Tags = updateDto.Tags;
        meetup.StartTime = updateDto.StartTime;
        meetup.EndTime = updateDto.EndTime;
        context.SaveChanges();

        var updatedDto = new UpdatedMeetupDto
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Tags = meetup.Tags,
            StartTime = meetup.StartTime,
            EndTime = meetup.EndTime,
            OrganizerId = meetup.OrganizerId
        };
        return Ok(updatedDto);
    }
}
