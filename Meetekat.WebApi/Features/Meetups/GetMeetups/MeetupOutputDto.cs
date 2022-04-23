namespace Meetekat.WebApi.Features.Meetups.GetMeetups;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class MeetupOutputDto
{
    [Required]
    public Guid Id { get; init; }
    
    [Required]
    public string Title { get; init; }
    
    [Required]
    public string Description { get; init; }
    
    [Required]
    public ICollection<string> Tags { get; init; }
    
    [Required]
    public DateTime StartTime { get; init; }
    
    [Required]
    public DateTime EndTime { get; init; }
    
    [Required]
    public string Organizer { get; init; }
}
