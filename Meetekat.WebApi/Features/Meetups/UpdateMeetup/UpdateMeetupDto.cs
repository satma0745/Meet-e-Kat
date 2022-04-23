namespace Meetekat.WebApi.Features.Meetups.UpdateMeetup;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Meetekat.WebApi.Validation;

[UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
public class UpdateMeetupDto
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
    
    [NotEmpty]
    public DateTime StartTime { get; set; }
    
    [NotEmpty]
    public DateTime EndTime { get; set; }
    
    [Required]
    [MaxLength(80)]
    public string Organizer { get; set; }
}
