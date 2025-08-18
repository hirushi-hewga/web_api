using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using web_api.BLL.DTOs.Cars;
using web_api.BLL.DTOs.Manufactures;
using web_api.BLL.Services;
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

            var response = await _carService.CreateAsync(dto);

            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] CarUpdateDto dto)
        {
            var validResult = await _carUpdateValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var response = await _carService.UpdateAsync(dto);

            return CreateActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (!ValidateId(id, out var message))
                return BadRequest(message);

            var response = await _carService.DeleteAsync(id);
            return CreateActionResult(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _carService.GetAllAsync();
            return CreateActionResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            if (!ValidateId(id, out var message))
                return BadRequest(message);

            var response = await _carService.GetByIdAsync(id);
            return CreateActionResult(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetPagedAsync(
            [FromQuery] int? year,
            [FromQuery] string? manufacture,
            [FromQuery] string? gearbox,
            [FromQuery] string? color,
            [FromQuery] string? model,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var response = await _carService.GetPagedAsync(year, manufacture, gearbox, color, model, pageNumber, pageSize);
            return CreateActionResult(response);
        }
    }
}