using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.Role;
using web_api.BLL.Services.Role;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : AppController
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<RoleDto> _roleValidator;

        public RoleController(IRoleService roleService, IValidator<RoleDto> roleValidator)
        {
            _roleService = roleService;
            _roleValidator = roleValidator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);
            
            var role = await _roleService.GetByIdAsync(id);
            
            return role == null ? BadRequest("Role not found") : Ok(role);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoleDto dto)
        {
            var validResult = await _roleValidator.ValidateAsync(dto);
            if (!validResult.IsValid)
                return BadRequest(validResult);
            
            var result = await _roleService.CreateAsync(dto);
            return result ? Ok("Role created") : BadRequest("Role not created");
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
            
            var result = await _roleService.UpdateAsync(dto);
            return result ? Ok("Role updated") : BadRequest("Role not updated");
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);

            var result = await _roleService.DeleteAsync(id);
            
            return result ? Ok("Role deleted") : BadRequest("Role not deleted");
        }
    }
}