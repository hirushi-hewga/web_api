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
        private readonly IValidator<ManufactureCreateDto> _manufactureCreateValidator;
        private readonly IValidator<ManufactureUpdateDto> _manufactureUpdateValidator;
        private readonly IManufactureService _manufactureService;

        public ManufactureController(IValidator<ManufactureCreateDto> manufactureCreateValidator,
                                     IValidator<ManufactureUpdateDto> manufactureUpdateValidator,
                                     IManufactureService manufactureService)
        {
            _manufactureCreateValidator = manufactureCreateValidator;
            _manufactureUpdateValidator = manufactureUpdateValidator;
            _manufactureService = manufactureService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ManufactureCreateDto dto)
        {
            var validResult = await _manufactureCreateValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var result = await _manufactureService.CreateAsync(dto);

            return result ? Ok($"{dto.Name} created") : BadRequest("Manufacture not created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ManufactureUpdateDto dto)
        {
            var validResult = await _manufactureUpdateValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var result = await _manufactureService.UpdateAsync(dto);

            return result ? Ok("Manufacture updated") : BadRequest("Manufacture not updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (!ValidateId(id, out var message))
                return BadRequest(message);

            var result = await _manufactureService.DeleteAsync(id);
            return result ? Ok("Manufacture deleted") : BadRequest("Manufacture not deleted");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var dtos = await _manufactureService.GetAllAsync();
            return Ok(dtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            if (!ValidateId(id, out var message))
                return BadRequest(message);

            var dto = await _manufactureService.GetByIdAsync(id);
            return dto == null ? BadRequest("Manufacture not found") : Ok(dto);
        }
    }
}
