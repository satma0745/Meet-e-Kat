namespace Meetekat.WebApi.Features.Meetups.RegisterNewMeetup;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

[UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
public class RegisterMeetupDto
{
    [Required]
    [MaxLength(120)]
    public string Title { get; set; }

    [Required]
    [MaxLength(10000)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(7)]
    public ICollection<string> Tags { get; set; }
    
    // TODO: This field is still not really required.
    [Required]
    public DateTime StartTime { get; set; }
    
    // TODO: This field is still not really required.
    [Required]
    public DateTime EndTime { get; set; }
    
    [Required]
    [MaxLength(80)]
    public string Organizer { get; set; }
}
