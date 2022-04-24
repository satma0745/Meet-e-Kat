namespace Meetekat.WebApi.Entities.Users;

using System.Collections.Generic;

public class Guest : User
{
    public override string Role => nameof(Guest);
    
    // This navigation property is required for configuring many-to-many relationship.
    // ReSharper disable once UnusedMember.Global
    public ICollection<Meetup> MeetupsSignedUpFor { get; set; }
}
