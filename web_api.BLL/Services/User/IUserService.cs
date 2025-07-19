using web_api.BLL.DTOs.User;

namespace web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<bool> CreateAsync(UserCreateDto dto);
        Task<bool> UpdateAsync(UserUpdateDto dto);
        Task<bool> DeleteAsync(string id);
        Task<UserDto?> GetByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAllAsync();
    }
}