namespace Meetekat.WebApi.Entities.Users;

using System.Collections.Generic;
using Meetekat.WebApi.Entities.Meetups;

public class Organizer : User
{
    public override string Role => nameof(Organizer);
    
    public ICollection<Meetup> OrganizedMeetups { get; set; }
}
