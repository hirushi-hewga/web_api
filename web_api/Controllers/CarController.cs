using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.Cars;
using web_api.BLL.DTOs.Manufactures;
using web_api.BLL.Services.Cars;
using web_api.BLL.Services.Manufactures;
using web_api.DAL;
using web_api.DAL.Entities;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : AppController
    {
        private readonly IValidator<CarCreateDto> _carCreateValidator;
        private readonly IValidator<CarUpdateDto> _carUpdateValidator;
        private readonly ICarService _carService;

        public CarController(IValidator<CarCreateDto> carCreateValidator,
                                     IValidator<CarUpdateDto> carUpdateValidator,
                                     ICarService carService)
        {
            _carService = carService;
            _carCreateValidator = carCreateValidator;
            _carUpdateValidator = carUpdateValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CarCreateDto dto)
        {
            var validResult = await _carCreateValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var result = await _carService.CreateAsync(dto);

            return result ? Ok($"Car created") : BadRequest("Car not created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] CarUpdateDto dto)
        {
            var validResult = await _carUpdateValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var result = await _carService.UpdateAsync(dto);

            return result ? Ok("Car updated") : BadRequest("Car not updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (!ValidateId(id, out var message))
                return BadRequest(message);

            var result = await _carService.DeleteAsync(id);
            return result ? Ok("Car deleted") : BadRequest("Car not deleted");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var dtos = await _carService.GetAllAsync();
            return Ok(dtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            if (!ValidateId(id, out var message))
                return BadRequest(message);

            var dto = await _carService.GetByIdAsync(id);
            return dto == null ? BadRequest("Car not found") : Ok(dto);
        }
    }
}