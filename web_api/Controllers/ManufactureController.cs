using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.Manufactures;
using web_api.BLL.DTOs.User;
using web_api.BLL.Services.Manufactures;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/manufacture")]
    [Authorize(Roles = "admin,manufactureManager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

            var response = await _manufactureService.CreateAsync(dto);

            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ManufactureUpdateDto dto)
        {
            var validResult = await _manufactureUpdateValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var response = await _manufactureService.UpdateAsync(dto);

            return CreateActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (!ValidateId(id, out var message))
                return BadRequest(message);

            var response = await _manufactureService.DeleteAsync(id);
            return CreateActionResult(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(string? id)
        {
            var response = string.IsNullOrEmpty(id)
                ? await _manufactureService.GetAllAsync()
                : await _manufactureService.GetByIdAsync(id);

            return CreateActionResult(response);
        }
    }
}
