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

            var result = await _userService.CreateAsync(dto);
            return result ? Ok("User created") : BadRequest("User not created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto dto)
        {
            var validResult = await _userUpdateValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);

            var result = await _userService.UpdateAsync(dto);
            return result ? Ok("User updated") : BadRequest("User not updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);
            
            var result = await _userService.DeleteAsync(id);
            return result ? Ok("User deleted") : BadRequest("User not deleted");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);
            
            var user = await _userService.GetByIdAsync(id);
            return user == null ? BadRequest("User not found") : Ok(user);
        }
    }
}