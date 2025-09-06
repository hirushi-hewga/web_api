using web_api.BLL.DTOs.Account;
using web_api.BLL.DTOs.User;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        Task<ServiceResponse> LoginAsync(LoginDto dto);
        Task<ServiceResponse> RegisterAsync(RegisterDto dto);
        Task<bool> EmailConfirmAsync(string id, string token);
        Task<bool> SendEmailConfirmAsync(string userId);
    }
}