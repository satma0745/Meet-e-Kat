namespace Meetekat.WebApi.Entities.Users;

using System.Collections.Generic;
using Meetekat.WebApi.Entities.Meetups;

public class Guest : User
{
    public override string Role => nameof(Guest);
    
    public ICollection<Meetup> MeetupsSignedUpFor { get; set; }
}
