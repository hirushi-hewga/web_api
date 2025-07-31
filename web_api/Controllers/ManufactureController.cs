using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.Manufactures;
using web_api.BLL.DTOs.User;
using web_api.BLL.Services.Manufactures;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/manufacture")]
    public class ManufactureController : AppController
    {
        private readonly IValidator<ManufactureDto> _manufactureValidator;
        private readonly IManufactureService _manufactureService;

        public ManufactureController(IValidator<ManufactureDto> manufactureValidator, IManufactureService manufactureService)
        {
            _manufactureValidator = manufactureValidator;
            _manufactureService = manufactureService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ManufactureDto dto)
        {
            var validResult = await _manufactureValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var result = await _manufactureService.CreateAsync(dto);

            return result ? Ok("Manufacture created") : BadRequest("Manufacture not created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ManufactureDto dto)
        {
            var validResult = await _manufactureValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var result = await _manufactureService.UpdateAsync(dto);

            return result ? Ok("Manufacture created") : BadRequest("Manufacture not created");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);

            var result = await _manufactureService.DeleteAsync(id);
            return result ? Ok("Manufacture deleted") : BadRequest("Manufacture not deleted");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _manufactureService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            var isValidId = ValidateId(id, out var message);
            if (!isValidId)
                return BadRequest(message);

            var user = await _manufactureService.GetByIdAsync(id);
            return user == null ? BadRequest("Manufacture not found") : Ok(user);
        }
    }
}
