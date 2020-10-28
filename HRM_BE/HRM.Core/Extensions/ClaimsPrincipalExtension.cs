using IdentityModel;
using System.Security.Claims;

namespace HRM.Core.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(JwtClaimTypes.Subject);
        }
    }
}
