using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories.Jwt
{
    public interface IJwtRepository
        : IGenericRepository<RefreshToken, string>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}
