using web_api.BLL.DTOs.Account;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        Task<AppUser> LoginAsync(LoginDto dto);
    }
}