using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Jwt
{
    public interface IJwtService
    {
        Task<string> GetJwtTokenAsync(AppUser user);
    }
}
