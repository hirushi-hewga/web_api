using web_api.BLL.DTOs.Role;

namespace web_api.BLL.Services.Role
{
    public interface IRoleService
    {
        Task<ServiceResponse> CreateAsync(RoleDto dto);
        Task<ServiceResponse> UpdateAsync(RoleDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetAllAsync();
    }
}