using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs;
using web_api.BLL.DTOs.Account;
using web_api.BLL.Services.Account;
using web_api.BLL.Services.Jwt;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : AppController
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IValidator<LoginDto> loginValidator, IValidator<RegisterDto> registerValidator, IJwtService jwtService)
        {
            _accountService = accountService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var validResult = await _loginValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);
            
            var response = await _accountService.LoginAsync(dto);
            return CreateActionResult(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var validResult = await _registerValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);
            
            var response = await _accountService.RegisterAsync(dto);
            return CreateActionResult(response);
        }

        [HttpGet("emailConfirm")]
        public async Task<IActionResult> EmailConfirmAsync(string? id, string? t)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t))
                return NotFound();

            var result = await _accountService.EmailConfirmAsync(id, t);
            
            if (!result)
                return NotFound();

            return Redirect("https://google.com");
        }

        [HttpGet("sendEmailConfirm")]
        public async Task<IActionResult> SendEmailConfirmAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();
            
            var result = await _accountService.SendEmailConfirmAsync(userId);
            
            return result ? Ok() : BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokensAsync(JwtTokensDto dto)
        {
            var response = await _jwtService.RefreshTokensAsync(dto);

            return CreateActionResult(response);
        }
    }
}