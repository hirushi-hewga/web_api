using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api.BLL.DTOs
{
    public class JwtTokensDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
