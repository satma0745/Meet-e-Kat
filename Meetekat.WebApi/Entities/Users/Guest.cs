namespace Meetekat.WebApi.Entities.Users;

public class Guest : User
{
    public override UserRole Role => UserRole.Guest;
}
