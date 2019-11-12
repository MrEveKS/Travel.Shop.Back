using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Travel.Shop.Back.Common.Domain.Managers;

namespace Travel.Shop.Back.Services
{
    public class AutorizationService : IAutorizationService
    {
        private readonly TimeSpan REMEMBER_TIME = TimeSpan.FromDays(14);

        private readonly TimeSpan NOT_REMEMBER_TIME = TimeSpan.FromHours(1);

        private readonly IConfiguration _configuration;

        private readonly UserManager<Manager> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AutorizationService(IConfiguration configuration,
            UserManager<Manager> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;

            _userManager = userManager;

            _roleManager = roleManager;
        }


        public async Task<string> GetToken(Manager manager, bool rememberMe)
        {
            var claims = await GetValidClaims(manager);

            var key = _configuration["AuthOptions:KEY"].GetSymmetricSecurityKey();

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var addTime = rememberMe ? REMEMBER_TIME : NOT_REMEMBER_TIME;

            var token = new JwtSecurityToken(_configuration["AuthOptions:ISSUER"],
              _configuration["AuthOptions:AUDIENCE"],
              claims,
              expires: DateTime.UtcNow.Add(addTime),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetValidClaims(Manager manager)
        {
            IdentityOptions options = new IdentityOptions();

            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, manager.UserName),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(options.ClaimsIdentity.UserIdClaimType, manager.Id.ToString()),
              new Claim(options.ClaimsIdentity.UserNameClaimType, manager.UserName)
            };

            var userClaims = await _userManager.GetClaimsAsync(manager);

            var userRoles = await _userManager.GetRolesAsync(manager);

            claims.AddRange(userClaims);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }
    }
}
