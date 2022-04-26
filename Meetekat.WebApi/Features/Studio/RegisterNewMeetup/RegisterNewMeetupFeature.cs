namespace Meetekat.WebApi.Features.Studio.RegisterNewMeetup;

using System;
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

public class RegisterNewMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public RegisterNewMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Studio)]
    [HttpPost("/api/studio/register-new-meetup")]
    [Authorize(Roles = nameof(Organizer))]
    [SwaggerOperation("Register a new meetup.")]
    [SwaggerResponse(StatusCodes.Status201Created, "A new meetup is registered successfully.", typeof(RegisteredMeetupDto))]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] RegisterMeetupDto registerDto)
    {
        var organizerExists = await context.Organizers.AnyAsync(organizer => organizer.Id == Caller.UserId);
        if (!organizerExists)
        {
            // Can happen if deleted Organizer tries to register a new Meetup (if the Access Token hasn't yet expired).
            return Unauthorized();
        }

        // Retrieve already existing Tags and create lacking ones.
        var persistedTags = await context.Tags
            .Where(tag => registerDto.Tags.Contains(tag.Name))
            .ToListAsync();
        var newTags = registerDto.Tags
            .Where(tagName => persistedTags.All(persistedTag => persistedTag.Name != tagName))
            .Select(tagName => new Tag { Id = Guid.NewGuid(), Name = tagName })
            .ToList();
        var tags = persistedTags.Concat(newTags).ToList();
        
        var meetup = new Meetup
        {
            Id = Guid.NewGuid(),
            Title = registerDto.Title,
            Description = registerDto.Description,
            Tags = tags,
            StartTime = registerDto.StartTime,
            EndTime = registerDto.EndTime,
            OrganizerId = Caller.UserId
        };
        context.Meetups.Add(meetup);
        await context.SaveChangesAsync();

        var registeredDto = new RegisteredMeetupDto
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Tags = meetup.Tags.Select(tag => tag.Name),
            StartTime = meetup.StartTime,
            EndTime = meetup.EndTime,
            OrganizerId = meetup.OrganizerId
        };
        return Created(registeredDto);
    }
}
