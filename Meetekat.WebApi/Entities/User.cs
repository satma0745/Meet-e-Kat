namespace Meetekat.WebApi.Entities;

using System;
using System.Collections.Generic;

public class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
