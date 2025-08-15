using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using web_api.BLL.DTOs.User;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        
        public async Task<ServiceResponse> CreateAsync(UserCreateDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ServiceResponse("Користувач із таким email вже існує");

            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return new ServiceResponse("Користувач із таким ім'ям вже існує");

            var entity = _mapper.Map<AppUser>(dto);
            
            var result = await _userManager.CreateAsync(entity, dto.Password);
            if (result.Succeeded)
                return new ServiceResponse("Користувача успішно створено", true);

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> UpdateAsync(UserUpdateDto dto)
        {
            if (await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id != dto.Id && u.NormalizedUserName == dto.UserName.ToUpper()) != null)
                return new ServiceResponse("Користувач із таким email вже існує");

            if (await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id != dto.Id && u.NormalizedEmail == dto.Email.ToUpper()) != null)
                return new ServiceResponse("Користувач із таким ім'ям вже існує");
            
            var entity = await _userManager.FindByIdAsync(dto.Id);
            _mapper.Map(entity, dto);

            if (!string.IsNullOrEmpty(dto.Password) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                var hasher = new PasswordHasher<AppUser>();
                var verifyResult = hasher.VerifyHashedPassword(entity, entity.PasswordHash, dto.Password);
                if (verifyResult == PasswordVerificationResult.Failed)
                    return new ServiceResponse("Неправильний поточний пароль");

                var removePasswordResult = await _userManager.RemovePasswordAsync(entity);
                if (!removePasswordResult.Succeeded) 
                    return new ServiceResponse($"Помилка видалення пароля: {string.Join(", ", removePasswordResult.Errors.Select(e => e.Description))}");

                var addPasswordResult = await _userManager.AddPasswordAsync(entity, dto.NewPassword);
                if (!addPasswordResult.Succeeded) 
                    return new ServiceResponse($"Помилка встановлення нового пароля: {string.Join(", ", addPasswordResult.Errors.Select(e => e.Description))}");
            }
            
            var result = await _userManager.UpdateAsync(entity);
            if (result.Succeeded)
                return new ServiceResponse("Користувача успішно оновлено", true);

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);
            
            if (entity == null)
                return new ServiceResponse("Користувача не знайдено");

            var result = await _userManager.DeleteAsync(entity);

            if (result.Succeeded)
                return new ServiceResponse("Користувача успішно видалено", true);

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);

            if (entity == null)
                return new ServiceResponse("Користувача не знайдено");

            var dto = _mapper.Map<UserDto>(entity);

            return new ServiceResponse("Користувача отримано", true, dto);
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<UserDto>>(entities);
            
            return new ServiceResponse("Користувачів отримано", true, dtos);
        }
    }
}