namespace Meetekat.WebApi.Features.Auth.RegisterNewUser;

using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class RegisteredUserDto
{
    /// <summary>User's immutable unique identifier.</summary>
    /// <example>13371337-1337-1337-1337-133713371337</example>
    [Required]
    public Guid Id { get; init; }
    
    /// <summary>User's nickname, part of the signing credentials.</summary>
    /// <example>satma0745</example>
    [Required]
    public string Username { get; init; }
}
