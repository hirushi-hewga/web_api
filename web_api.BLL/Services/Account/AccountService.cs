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
            var encodedToken = Uri.EscapeDataString(token);
            string messageBody = $@"
                <a href='http://localhost:5014/api/account/emailConfirm?id={user.Id}&t={encodedToken}'>
                    Підтвердити пошту
                </a>";

            await _emailService.SendMailAsync(user.Email, "Email confirm", messageBody, true);
            
            return user;
        }

        public async Task<bool> EmailConfirmAsync(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            var decodedToken = Uri.UnescapeDataString(token);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            return result.Succeeded;
        }
    }
}