using HRM.Core.Models.Users;
using HRM.Core.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using System.Data;
using System;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HRM.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSetting;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtSettings> jwtSetting)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSetting = jwtSetting.Value;
        }

        private static IEnumerable<Claim> GetTokenClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.NickName, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email)
            };
        }

        private async Task<IEnumerable<Claim>> GetRoleClaims(User user)
        {
            var claims = new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);
            foreach(var rolename in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rolename));
                claims.Add(new Claim(JwtClaimTypes.Role, rolename));

                var role = await _roleManager.FindByNameAsync(rolename);
                if (role == null) continue;

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }

        public async Task<JwtSecurityToken> GetJwtTokenAsync(User user, params Claim[] extraClaims)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roleClaims = await GetRoleClaims(user);
            var claims = GetTokenClaims(user).Union(userClaims).Union(roleClaims).Union(extraClaims);

            return new JwtSecurityToken(
                null,
                null,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSetting.Expiration),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey)),
                    SecurityAlgorithms.HmacSha256)
                );
        }
    }
}
