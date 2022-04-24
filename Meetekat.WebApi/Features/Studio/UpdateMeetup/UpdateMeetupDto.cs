namespace Meetekat.WebApi.Features.Studio.UpdateMeetup;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Meetekat.WebApi.Seedwork.Validation;

[UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
public class UpdateMeetupDto
{
    /// <summary>Meetup Title.</summary>
    /// <example>Microsoft's naming issues.</example>
    [Required]
    [MaxLength(120)]
    public string Title { get; set; }

    /// <summary>Meetup Description.</summary>
    /// <example>Today we will talk about Microsoft's famous issues ...</example>
    [Required]
    [MaxLength(10000)]
    public string Description { get; set; }
    
    /// <summary>Meetup Tags, Keywords.</summary>
    /// <example>["microsoft", ".net", "dotnet"]</example>
    [Required]
    [MaxLength(7)]
    public ICollection<string> Tags { get; set; }
    
    /// <summary>Date and Time when the Meetup starts.</summary>
    /// <example>2022-04-22T16:30:00.000Z</example>
    [NotEmpty]
    public DateTime StartTime { get; set; }
    
    /// <summary>Date and Time when the Meetup ends.</summary>
    /// <example>2022-04-22T21:30:00.000Z</example>
    [NotEmpty]
    public DateTime EndTime { get; set; }
}
