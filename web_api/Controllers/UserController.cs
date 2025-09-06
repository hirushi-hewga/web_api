using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.User;
using web_api.BLL.Services.User;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : AppController
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserCreateDto> _userCreateValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateValidator;

        public UserController(IUserService userService, 
            IValidator<UserCreateDto> userCreateValidator, 
            IValidator<UserUpdateDto> userUpdateValidator)
        {
            _userService = userService;
            _userCreateValidator = userCreateValidator;
            _userUpdateValidator = userUpdateValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateDto dto)
        {
            var validResult = await _userCreateValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);

            var response = await _userService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto dto)
        {
            var validResult = await _userUpdateValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);

            var response = await _userService.UpdateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);
            
            var response = await _userService.DeleteAsync(id);
            return CreateActionResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? id)
        {
            var response = string.IsNullOrEmpty(id)
                ? await _userService.GetAllAsync()
                : await _userService.GetByIdAsync(id);

            return CreateActionResult(response);
        }
    }
}