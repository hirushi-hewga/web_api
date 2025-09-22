using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories.Jwt
{
    public class JwtRepository
        : GenericRepository<RefreshToken, string>, IJwtRepository
    {
        private readonly AppDbContext _context;
        public JwtRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }
    }
}
