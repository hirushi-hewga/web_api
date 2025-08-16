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

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedAsync(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _carService.GetPagedAsync(pageNumber, pageSize);
            return CreateActionResult(response);
        }

        [HttpGet("paged/by-year")]
        public async Task<IActionResult> GetPagedByYearAsync(int? year, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _carService.GetPagedByYearAsync(pageNumber, pageSize, year);
            return CreateActionResult(response);
        }

        [HttpGet("paged/by-manufacture")]
        public async Task<IActionResult> GetPagedByManufactureAsync(string? manufacture, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _carService.GetPagedByManufactureAsync(pageNumber, pageSize, manufacture);
            return CreateActionResult(response);
        }

        [HttpGet("paged/by-gearbox")]
        public async Task<IActionResult> GetPagedByGearBoxAsync(string? gearbox, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _carService.GetPagedByGearBoxAsync(pageNumber, pageSize, gearbox);
            return CreateActionResult(response);
        }

        [HttpGet("paged/by-color")]
        public async Task<IActionResult> GetPagedByColorAsync(string? color, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _carService.GetPagedByColorAsync(pageNumber, pageSize, color);
            return CreateActionResult(response);
        }

        [HttpGet("paged/by-model")]
        public async Task<IActionResult> GetPagedByModelAsync(string? model, int pageNumber = 1, int pageSize = 10)
        {
            var response = await _carService.GetPagedByModelAsync(pageNumber, pageSize, model);
            return CreateActionResult(response);
        }
    }
}