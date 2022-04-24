namespace Meetekat.WebApi.Features.Meetups.GetMeetups;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class MeetupDto
{
    /// <summary>Meetup ID.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [Required]
    public Guid Id { get; init; }
    
    /// <summary>Meetup Title.</summary>
    /// <example>Microsoft's naming issues.</example>
    [Required]
    public string Title { get; init; }
    
    /// <summary>Meetup Description.</summary>
    /// <example>Today we will talk about Microsoft's famous issues ...</example>
    [Required]
    public string Description { get; init; }
    
    /// <summary>Meetup Tags, Keywords.</summary>
    /// <example>["microsoft", ".net", "dotnet"]</example>
    [Required]
    public ICollection<string> Tags { get; init; }
    
    /// <summary>Date and Time when the Meetup starts.</summary>
    /// <example>2022-04-22T16:30:00.000Z</example>
    [Required]
    public DateTime StartTime { get; init; }
    
    /// <summary>Date and Time when the Meetup ends.</summary>
    /// <example>2022-04-22T21:30:00.000Z</example>
    [Required]
    public DateTime EndTime { get; init; }
    
    /// <summary>ID of the Meetup Organizer.</summary>
    /// <example>13371337-1337-1337-1337-133713371337</example>
    [Required]
    public Guid OrganizerId { get; init; }
    
    /// <summary>The number of Guests who decided to sign up for a meetup.</summary>
    /// <example>17</example>
    [Required]
    public int SignedUpGuestsCount { get; init; }
}
