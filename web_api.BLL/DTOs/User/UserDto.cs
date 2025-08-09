using FluentValidation;
using web_api.BLL.DTOs.Role;

namespace web_api.BLL.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public IEnumerable<RoleDto> Roles { get; set; } = [];
    }
}