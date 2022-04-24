namespace Meetekat.WebApi.Entities.Users;

using System;
using System.Collections.Generic;

public abstract class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public abstract UserRole Role { get; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
