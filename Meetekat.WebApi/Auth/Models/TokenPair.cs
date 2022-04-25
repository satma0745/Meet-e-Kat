namespace Meetekat.WebApi.Auth.Models;

public class TokenPair
{
    public string AccessToken { get; init; }
    
    public string RefreshToken { get; init; }
}
