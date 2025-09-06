using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using web_api.BLL.DTOs.Account;
using web_api.BLL.DTOs.User;
using web_api.BLL.Services.Email;
using web_api.BLL.Services.Jwt;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, IJwtService jwtService, IEmailService emailService, IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        
        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            AppUser? user = null;
            
            if (dto.Login.Contains('@'))
                user = await _userManager.FindByEmailAsync(dto.Login);
            else
                user = await _userManager.FindByNameAsync(dto.Login);

            if (user == null)
                return new ServiceResponse($"Користувача з логіном '{dto.Login}' не знайдено");
            
            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!result)
                return new ServiceResponse("Пароль вказано невірно");

            var jwtToken = await _jwtService.GetJwtTokenAsync(user);
            return new ServiceResponse("Успішний вхід", true, jwtToken);
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ServiceResponse($"Пошта '{dto.Email}' вже використовується");
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return new ServiceResponse($"Ім'я користувача '{dto.UserName}' вже використовується");

            var user = _mapper.Map<AppUser>(dto);
            
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return new ServiceResponse(result.Errors.First().Description);
            
            if (result.Succeeded)
                result = await _userManager.AddToRoleAsync(user, "user");
            
            if (!result.Succeeded)
                return new ServiceResponse(result.Errors.First().Description);

            await SendEmailConfirmAsync(user.Id);

            var jwtToken = await _jwtService.GetJwtTokenAsync(user);
            return new ServiceResponse("Успішна реєстрація", true, jwtToken);
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

        public async Task<bool> SendEmailConfirmAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(token);
                string messageBody = $@"
                    <a href='http://localhost:5014/api/account/emailConfirm?id={user.Id}&t={encodedToken}'>
                        Підтвердити пошту
                    </a>";

                await _emailService.SendMailAsync(user.Email, "Email confirm", messageBody, true);
            
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}