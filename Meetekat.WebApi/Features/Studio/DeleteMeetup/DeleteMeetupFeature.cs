namespace Meetekat.WebApi.Features.Studio.DeleteMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

public class DeleteMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public DeleteMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Studio)]
    [HttpPost("/api/studio/delete-meetup")]
    [Authorize(Roles = nameof(Organizer))]
    [SwaggerOperation("Delete a Meetup with matching ID.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "A Meetup with the specified ID was deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Only a Meetup's direct Organizer can delete the Meetup.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "A Meetup with the specified ID doesn't exist.")]
    public async Task<IActionResult> DeleteMeetup([FromQuery] [Required] Guid meetupId)
    {
        var meetup = await context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }
        
        var organizerExists = await context.Organizers.AnyAsync(organizer => organizer.Id == Caller.UserId);
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
        await context.SaveChangesAsync();

        return NoContent();
    }
}
