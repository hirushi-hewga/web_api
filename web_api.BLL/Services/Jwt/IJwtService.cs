using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateRefreshToken();
        Task<JwtSecurityToken> GenerateAccessTokenAsync(AppUser user);
        Task<JwtTokensDto?> GenerateTokensAsync(AppUser user);
        Task<ServiceResponse> RefreshTokensAsync(JwtTokensDto dto);
        //Task<string> GetJwtTokenAsync(AppUser user);
    }
}
