namespace Meetekat.WebApi.Entities.Meetups;

using System;
using System.Collections.Generic;

public class Tag
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public ICollection<Meetup> TaggedMeetups { get; set; }
}
