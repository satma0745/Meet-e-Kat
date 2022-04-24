namespace Meetekat.WebApi.Features.Meetups.DeleteMeetup;

using System;
using System.Linq;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class DeleteMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public DeleteMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Meetups)]
    [HttpDelete("/api/meetups/{meetupId:guid}")]
    [Authorize(Roles = nameof(Organizer))]
    [SwaggerOperation("Delete a Meetup with matching ID.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "A Meetup with the specified ID was deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Only a Meetup's direct Organizer can delete the Meetup.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "A Meetup with the specified ID doesn't exist.")]
    public IActionResult DeleteMeetup([FromRoute] Guid meetupId)
    {
        var meetup = context.Meetups.SingleOrDefault(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }
        
        var organizerExists = context.Organizers.Any(organizer => organizer.Id == Caller.UserId);
        if (!organizerExists)
        {
            // Can happen if deleted Organizer tries to delete a Meetup (if the Access Token hasn't yet expired).
            return Unauthorized();
        }
        if (meetup.OrganizerId != Caller.UserId)
        {
            // Only a Meetup's direct Organizer can delete the Meetup.
            return Forbidden();
        }

        context.Meetups.Remove(meetup);
        context.SaveChanges();

        return NoContent();
    }
}
