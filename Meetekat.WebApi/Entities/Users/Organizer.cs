namespace Meetekat.WebApi.Entities.Users;

using System.Collections.Generic;

public class Organizer : User
{
    public override string Role => nameof(Organizer);
    
    public ICollection<Meetup> OrganizedMeetups { get; set; }
}
