using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FormManagementSystem.Extensions;

public static class ClaimsPrincipalExtensions
{
    
    public static Guid? GetPublicId(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirstValue(
            JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(claimValue, out var publicId)
            ? publicId
            : null;
    }
}