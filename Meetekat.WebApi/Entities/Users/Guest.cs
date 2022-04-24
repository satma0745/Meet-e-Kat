namespace Meetekat.WebApi.Entities.Users;

public class Guest : User
{
    public override string Role => nameof(Guest);
}
