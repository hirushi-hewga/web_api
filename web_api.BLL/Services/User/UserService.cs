using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using web_api.BLL.DTOs.User;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<bool> CreateAsync(UserCreateDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null
                || await _userManager.FindByNameAsync(dto.Username) != null)
                return false;

            var entity = new AppUser
            {
                Id = dto.Id,
                UserName = dto.Username,
                Email = dto.Email
            };
            
            var result = await _userManager.CreateAsync(entity, dto.Password);
            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(UserUpdateDto dto)
        {
            if (_userManager.Users.Where(u => u.Id != dto.Id)
                .FirstOrDefault(u => u.NormalizedUserName == dto.Username.ToUpper()
                    || u.NormalizedEmail == dto.Email.ToUpper()) != null)
                return false;
            
            var user = await _userManager.FindByIdAsync(dto.Id);
            user.UserName = dto.Username;
            user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Password) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                var hasher = new PasswordHasher<AppUser>();
                var verifyResult = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
                if (verifyResult == PasswordVerificationResult.Failed)
                    return false;
                
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded) 
                    return false;
                
                var addPasswordResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);
                if (!addPasswordResult.Succeeded) 
                    return false;
            }
            
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);
            
            if (entity != null)
            {
                var result = await _userManager.DeleteAsync(entity);
                return result.Succeeded;
            }
            
            return false;
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);

            if (entity == null)
                return null;
            
            var dto = new UserDto
            { 
                Id = entity.Id, 
                Username = entity.UserName ?? "",
                Email = entity.Email ?? "",
            };
            
            return dto;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var entities = await _userManager.Users.ToListAsync();

            var dtos = entities
                .Select(e => new UserDto { Id = e.Id, Username = e.UserName ?? "", Email = e.Email ?? ""});
            
            return dtos;
        }
    }
}