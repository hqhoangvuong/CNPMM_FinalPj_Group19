using HRM.Core.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRM.Service.Services
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> GetJwtTokenAsync(User user, params Claim[] extraClaims);
    }
}
