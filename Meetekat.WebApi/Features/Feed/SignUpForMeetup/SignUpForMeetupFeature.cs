namespace Meetekat.WebApi.Features.Feed.SignUpForMeetup;

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

public class SignUpForMeetupFeature : FeatureBase
{
    private readonly ApplicationContext context;

    public SignUpForMeetupFeature(ApplicationContext context) =>
        this.context = context;

    [Tags(ApiSections.Feed)]
    [HttpPost("/api/feed/signup-for-meetup")]
    [Authorize(Roles = nameof(Guest))]
    [SwaggerOperation("Sign up for a specific Meetup with matching ID.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Successfully signed up for a Meetup.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Meetup with the specified ID doesn't exist.")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "You've already signed up for this specific meetup.")]
    public async Task<IActionResult> SignUpForMeetup([FromQuery] [Required] Guid meetupId)
    {
        var meetup = await context.Meetups
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        var currentUser = await context.Guests.SingleOrDefaultAsync(guest => guest.Id == Caller.UserId);
        if (currentUser is null)
        {
            // Can happen if deleted User tries to sign up for a meetup (if the Access Token hasn't yet expired).
            return Unauthorized();
        }

        if (meetup.SignedUpGuests.Contains(currentUser))
        {
            return Conflict();
        }

        meetup.SignedUpGuests.Add(currentUser);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
