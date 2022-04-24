namespace Meetekat.WebApi.Entities.Users;

public class Organizer : User
{
    public override string Role => nameof(Organizer);
}
