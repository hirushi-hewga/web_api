using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.Account;
using web_api.BLL.Services.Account;
using web_api.DAL.Entities;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var user = await _accountService.LoginAsync(dto);
            return user == null ? BadRequest("Incorrect login or password") : Ok(user);
        }
    }
}