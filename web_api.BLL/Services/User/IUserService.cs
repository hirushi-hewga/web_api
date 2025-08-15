using web_api.BLL.DTOs.User;

namespace web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<ServiceResponse> CreateAsync(UserCreateDto dto);
        Task<ServiceResponse> UpdateAsync(UserUpdateDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetAllAsync();
    }
}