namespace Meetekat.WebApi.Features.Meetups.DeleteMeetup;

using System;
using System.Linq;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class DeleteMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public DeleteMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags("Meetups")]
    [HttpGet("/api/meetups/{meetupId:guid}")]
    [SwaggerOperation("Delete a Meetup with matching ID.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "A Meetup with the specified ID was deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "A Meetup with the specified ID doesn't exist.")]
    public IActionResult DeleteMeetup([FromRoute] Guid meetupId)
    {
        var meetup = context.Meetups.SingleOrDefault(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        context.Meetups.Remove(meetup);
        context.SaveChanges();

        return NoContent();
    }
}
