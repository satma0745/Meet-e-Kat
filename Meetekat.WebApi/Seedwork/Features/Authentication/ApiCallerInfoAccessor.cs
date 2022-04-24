namespace Meetekat.WebApi.Seedwork.Features.Authentication;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

public class ApiCallerInfoAccessor
{
    public ApiCaller ApiCaller => apiCaller.Value;
    
    private readonly Lazy<ApiCaller> apiCaller;

    public ApiCallerInfoAccessor(IEnumerable<Claim> claims) =>
        apiCaller = new Lazy<ApiCaller>(() => ExtractCallerInfoFromClaims(claims));

    private static ApiCaller ExtractCallerInfoFromClaims(IEnumerable<Claim> claims)
    {
        try
        {
            var currentUserIdClaim = claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier);
            var currentUserId = Guid.Parse(currentUserIdClaim.Value);
            
            return new ApiCaller
            {
                UserId = currentUserId
            };
        }
        catch (Exception)
        {
            throw new ApiCallerNotAuthenticatedException();
        }
    }
}
