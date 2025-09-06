using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using web_api.BLL.DTOs.User;
using web_api.BLL.Services.Image;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, IImageService imageService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _imageService = imageService;
        }
        
        public async Task<ServiceResponse> CreateAsync(UserCreateDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ServiceResponse("Користувач із таким email вже існує");

            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return new ServiceResponse("Користувач із таким ім'ям вже існує");

            var entity = _mapper.Map<AppUser>(dto);

            if (dto.Image != null)
            {
                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.UsersPath);
                if (imageName != null)
                {
                    imageName = Path.Combine(Settings.UsersPath, imageName);
                }
                entity.Image = imageName;
            }
            
            var result = await _userManager.CreateAsync(entity, dto.Password);
            if (!result.Succeeded)
                return new ServiceResponse(result.Errors.First().Description);

            result = await _userManager.AddToRolesAsync(entity, dto.Roles);
            if (result.Succeeded)
                return new ServiceResponse("Користувача успішно створено", true);

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
                return new ServiceResponse("Користувача не знайдено");

            if (await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id != dto.Id && u.NormalizedUserName == dto.UserName.ToUpper()) != null)
                return new ServiceResponse("Користувач із таким email вже існує");

            if (await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id != dto.Id && u.NormalizedEmail == dto.Email.ToUpper()) != null)
                return new ServiceResponse("Користувач із таким ім'ям вже існує");
            
            var entity = _mapper.Map(dto, user);

            if (dto.Image != null)
            {
                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.UsersPath);
                if (imageName != null)
                {
                    imageName = Path.Combine(Settings.UsersPath, imageName);
                }

                if (!string.IsNullOrEmpty(entity.Image) && !string.IsNullOrEmpty(imageName))
                {
                    _imageService.DeleteImage(entity.Image);
                }

                entity.Image = imageName;
            }

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
            if (!result.Succeeded)
                return new ServiceResponse(result.Errors.First().Description);

            var userRoles = await _userManager.GetRolesAsync(entity);

            var deleteRoles = userRoles.Where(r => !dto.Roles.Contains(r));

            if (deleteRoles.Any())
            {
                var deleteRes = await _userManager.RemoveFromRolesAsync(entity, deleteRoles);
                if (!deleteRes.Succeeded)
                {
                    return new ServiceResponse(deleteRes.Errors.First().Description);
                }
            }

            var newRoles = dto.Roles.Where(r => !userRoles.Contains(r));
            if (newRoles.Any())
            {
                var addRes = await _userManager.AddToRolesAsync(entity, newRoles);
                if (!addRes.Succeeded)
                {
                    return new ServiceResponse(addRes.Errors.First().Description);
                }
            }

            return new ServiceResponse("Користувача успішно оновлено", true);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);
            
            if (entity == null)
                return new ServiceResponse("Користувача не знайдено");

            if (!string.IsNullOrEmpty(entity.Image))
                _imageService.DeleteImage(entity.Image);

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