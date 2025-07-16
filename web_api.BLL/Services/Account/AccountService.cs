using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using web_api.BLL.DTOs.Account;
using web_api.BLL.Services.Email;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }
        
        public async Task<AppUser> LoginAsync(LoginDto dto)
        {
            AppUser? user = null;
            
            if (dto.Login.Contains('@'))
                user = await _userManager.FindByEmailAsync(dto.Login);
            else
                user = await _userManager.FindByNameAsync(dto.Login);

            if (user == null)
                return null;
            
            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            
            return result ? user : null;
        }

        public async Task<AppUser?> RegisterAsync(RegisterDto dto)
        {
            // if (await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == dto.Email.ToUpper() 
            //     || x.NormalizedUserName == dto.Username.ToUpper()) != null)
            //     return null;
            
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return null;
            if (await _userManager.FindByNameAsync(dto.Username) != null)
                return null;

            var user = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };
            
            var result = await _userManager.CreateAsync(user, dto.Password);
            
            if (!result.Succeeded)
                return null;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string messageBody = $"<a href='https://localhost:7220/api/account/emailConfirm?id={user.Id}&t={token}'>Підтвердити пошту</а>";

            await _emailService.SendMailAsync("lo067803@gmail.com", "Email confirm", messageBody, true);
            
            return user;
        }

        public Task<bool> EmailConfirmAsync(string id, string token)
        {
            
            return false;
        }
    }
}