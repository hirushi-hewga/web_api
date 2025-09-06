using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public JwtService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> GetJwtTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("firstName", user.FirstName ?? ""),
                new Claim("lastName", user.LastName ?? ""),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? ""),
                new Claim("image", user.Image ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                var roleClaims = roles.Select(r => new Claim("role", r));
                claims.AddRange(roleClaims);
            }

            string? audience = _configuration["JwtSettings:Audience"];
            string? issuer = _configuration["JwtSettings:Issuer"];
            string? secretKey = _configuration["JwtSettings:SecretKey"];
            int expMinutes = int.Parse(_configuration["JwtSettings:ExpMinutes"] ?? "");

            if (audience == null || issuer == null || secretKey == null)
            {
                throw new ArgumentNullException("Jwt settings not found");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(expMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
