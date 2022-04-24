namespace Meetekat.WebApi.Features.Auth.RegisterNewOrganizer;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

[UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
public class RegisterOrganizerDto
{
    /// <summary>User's nickname, part of the signing credentials.</summary>
    /// <example>satma0745</example>
    [Required]
    [MinLength(6)]
    [MaxLength(24)]
    public string Username { get; set; }
    
    /// <summary>User's password, part of the signing credentials.</summary>
    /// <example>pa$$word</example>
    [Required]
    [MinLength(6)]
    [MaxLength(24)]
    public string Password { get; set; }
}
