using EmailingSystem.Core.Contracts.Services.Contracts;
using EmailingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationUser user;
        private readonly IConfiguration configuration;

        public TokenService(UserManager<ApplicationUser> userManager, ApplicationUser user, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.user = user;
            this.configuration = configuration;
        }

        public async Task<string> CreateTokenAsync()
        {
            var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var Roles = await userManager.GetRolesAsync(user);

            foreach (var role in Roles)
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? string.Empty));

            var Token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(double.Parse(configuration["JWT:DurationInMins"])),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
