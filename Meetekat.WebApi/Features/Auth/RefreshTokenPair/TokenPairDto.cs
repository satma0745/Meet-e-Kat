namespace Meetekat.WebApi.Features.Auth.RefreshTokenPair;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class TokenPairDto
{
    /// <summary>JWT Access Token.</summary>
    /// <example>jwt.access.token</example>
    [Required]
    public string AccessToken { get; init; }
    
    /// <summary>JWT Refresh Token</summary>
    /// <example>jwt.refresh.token</example>
    [Required]
    public string RefreshToken { get; init; }
}
