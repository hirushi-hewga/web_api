using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.Role;
using web_api.BLL.Services.Role;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/role")]
    [Authorize(Roles = "admin,manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : AppController
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<RoleDto> _roleValidator;

        public RoleController(IRoleService roleService, IValidator<RoleDto> roleValidator)
        {
            _roleService = roleService;
            _roleValidator = roleValidator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoleDto dto)
        {
            var validResult = await _roleValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);
            
            var response = await _roleService.CreateAsync(dto);
            return CreateActionResult(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] RoleDto dto)
        {
            var isValidId = ValidateId(dto.Id, out var message);
            if (!isValidId)
                return BadRequest(message);
            
            var validResult = await _roleValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);
            
            var response = await _roleService.UpdateAsync(dto);
            return CreateActionResult(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);

            var response = await _roleService.DeleteAsync(id ?? "");

            return CreateActionResult(response);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(string? id)
        {
            var response = string.IsNullOrEmpty(id)
                ? await _roleService.GetAllAsync()
                : await _roleService.GetByIdAsync(id);

            return CreateActionResult(response);
        }
    }
}