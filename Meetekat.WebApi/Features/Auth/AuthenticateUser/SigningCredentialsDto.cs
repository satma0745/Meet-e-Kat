namespace Meetekat.WebApi.Features.Auth.AuthenticateUser;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

[UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
public class SigningCredentialsDto
{
    /// <summary>User's nickname, part of the signing credentials.</summary>
    /// <example>satma0745</example>
    [Required]
    public string Username { get; set; }
    
    /// <summary>User's password, part of the signing credentials.</summary>
    /// <example>pa$$word</example>
    [Required]
    public string Password { get; set; }
}
