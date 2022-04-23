namespace Meetekat.WebApi.Features.Meetups.UpdateMeetup;

using System;
using System.Linq;
using System.Net.Mime;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Tags("Meetups")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UpdateMeetupFeature : ControllerBase
{
    private readonly ApplicationContext context;

    public UpdateMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [HttpPut("/api/meetups/{meetupId:guid}")]
    [SwaggerOperation("Update a Meetup with the matching ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "A Meetup with the specified ID was updated successfully.", typeof(UpdatedMeetupDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "A Meetup with the specified ID doesn't exist.")]
    public IActionResult UpdateMeetup([FromRoute] Guid meetupId, [FromBody] UpdateMeetupDto updateDto)
    {
        var meetup = context.Meetups.SingleOrDefault(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        meetup.Title = updateDto.Title;
        meetup.Description = updateDto.Description;
        meetup.Tags = updateDto.Tags;
        meetup.StartTime = updateDto.StartTime;
        meetup.EndTime = updateDto.EndTime;
        meetup.Organizer = updateDto.Organizer;
        context.SaveChanges();

        var updatedDto = new UpdatedMeetupDto
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Tags = meetup.Tags,
            StartTime = meetup.StartTime,
            EndTime = meetup.EndTime,
            Organizer = meetup.Organizer
        };
        return Ok(updatedDto);
    }
}
