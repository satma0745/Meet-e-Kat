namespace Meetekat.WebApi.Features.Studio.UpdateMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Meetekat.WebApi.Entities.Meetups;
using Meetekat.WebApi.Entities.Users;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

public class UpdateMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public UpdateMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Studio)]
    [HttpPost("/api/studio/update-meetup")]
    [Authorize(Roles = nameof(Organizer))]
    [SwaggerOperation("Update a Meetup with the matching ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "A Meetup with the specified ID was updated successfully.", typeof(UpdatedMeetupDto))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Only a Meetup's direct Organizer can update the Meetup.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "A Meetup with the specified ID doesn't exist.")]
    public async Task<IActionResult> UpdateMeetup([FromQuery] [Required] Guid meetupId, [FromBody] UpdateMeetupDto updateDto)
    {
        var meetup = await context.Meetups
            .Include(meetup => meetup.Tags)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }
        
        var organizerExists = await context.Organizers.AnyAsync(organizer => organizer.Id == Caller.UserId);
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
        
        // Retrieve already existing Tags and create lacking ones.
        var persistedTags = await context.Tags
            .Where(tag => updateDto.Tags.Contains(tag.Name))
            .ToListAsync();
        var newTags = updateDto.Tags
            .Where(tagName => persistedTags.All(persistedTag => persistedTag.Name != tagName))
            .Select(tagName => new Tag { Id = Guid.NewGuid(), Name = tagName })
            .ToList();
        var tags = persistedTags.Concat(newTags).ToList();

        meetup.Title = updateDto.Title;
        meetup.Description = updateDto.Description;
        meetup.Tags = tags;
        meetup.StartTime = updateDto.StartTime;
        meetup.EndTime = updateDto.EndTime;
        await context.SaveChangesAsync();

        var updatedDto = new UpdatedMeetupDto
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Tags = meetup.Tags.Select(tag => tag.Name),
            StartTime = meetup.StartTime,
            EndTime = meetup.EndTime,
            OrganizerId = meetup.OrganizerId
        };
        return Ok(updatedDto);
    }
}
