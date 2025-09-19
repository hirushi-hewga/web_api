using System.Linq.Expressions;
using web_api.BLL.DTOs.User;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<ServiceResponse> CreateAsync(UserCreateDto dto);
        Task<ServiceResponse> UpdateAsync(UserUpdateDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByUserNameAsync(string userName);
        Task<ServiceResponse> GetByEmailAsync(string email);
        Task<ServiceResponse> GetUserAsync(Expression<Func<AppUser, bool>> predicate);
    }
}