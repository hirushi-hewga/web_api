using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs;
using web_api.BLL.DTOs.Cars;
using web_api.BLL.DTOs.Manufactures;
using web_api.BLL.Services.Image;
using web_api.DAL;
using web_api.DAL.Entities;
using web_api.DAL.Repositories.Cars;
using web_api.DAL.Repositories.Manufactures;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace web_api.BLL.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly IManufactureRepository _manufactureRepository;
        private readonly ICarRepository _carRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IImageService imageService, IMapper mapper, IManufactureRepository manufactureRepository)
        {
            _manufactureRepository = manufactureRepository;
            _carRepository = carRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(CarCreateDto dto)
        {
            var entity = _mapper.Map<Car>(dto);

            if (!string.IsNullOrEmpty(dto.Manufacture))
            {
                entity.Manufacture = await _manufactureRepository.GetByNameAsync(dto.Manufacture);
            }

            if (dto.Images.Count() > 0)
            {
                string path = Path.Combine(Settings.CarsPath, entity.Id);
                _imageService.CreateDirectory(path);
                var carImages = await _imageService.SaveCarImagesAsync(dto.Images, path);
                entity.Images = carImages;
            }

            var result = await _carRepository.CreateAsync(entity);
            if (result)
                return new ServiceResponse($"Автомобіль '{dto.Brand} {dto.Model}' успішно створено", true);

            return new ServiceResponse("Не вдалося створити автомобіль");
        }

        public async Task<ServiceResponse> UpdateAsync(CarUpdateDto dto)
        {
            var entity = await _carRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (entity == null)
                return new ServiceResponse("Автомобіль не знайдено");

            entity = _mapper.Map(dto, entity);

            var path = Path.Combine(Settings.CarsPath, entity.Id);
            _imageService.DeleteDirectory(path);

            if (dto.Images != null && dto.Images.Any())
            {
                _imageService.CreateDirectory(path);
                var carImages = await _imageService.SaveCarImagesAsync(dto.Images, path);
                entity.Images = carImages;
            }

            var result = await _carRepository.UpdateAsync(entity);
            if (result)
                return new ServiceResponse($"Автомобіль '{entity.Brand} {entity.Model}' успішно оновлено", true);

            return new ServiceResponse("Не вдалося оновити автомобіль");
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _carRepository.GetByIdAsync(id);

            if (entity == null)
                return new ServiceResponse("Автомобіль не знайдено");

            if (entity.Images.Any())
            {
                var path = Path.Combine(Settings.CarsPath, entity.Id);
                _imageService.DeleteDirectory(path);
            }

            var result = await _carRepository.DeleteAsync(entity);
            if (result)
                return new ServiceResponse("Автомобіль видалено", true);

            return new ServiceResponse("Не вдалося видалити автомобіль");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<CarDto>>(entities);

            return new ServiceResponse("Автомобілі отримано", true, dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _carRepository.GetByIdAsync(id);

            if (entity == null)
                return new ServiceResponse("Автомобіль не знайдено");

            var dto = _mapper.Map<CarDto>(entity);

            return new ServiceResponse("Автомобіль отримано", true, dto);
        }

        private async Task<PagedResult<CarDto>> CreatePagedResultAsync(int pageNumber, int pageSize, IQueryable<Car> entities)
        {
            var totalCount = await entities.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var list = await entities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = _mapper.Map<IEnumerable<CarDto>>(list);

            var result = new PagedResult<CarDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages
            };

            return result;
        }

        public async Task<ServiceResponse> GetPagedAsync(int pageNumber, int pageSize)
        {
            var entities = _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture);

            var result = await CreatePagedResultAsync(pageNumber, pageSize, entities);
            return new ServiceResponse("Автомобілі отримано", true, result);
        }

        public async Task<ServiceResponse> GetPagedByYearAsync(int pageNumber, int pageSize, int? year)
        {
            if (year == null)
                return new ServiceResponse("Рік не вказано");

            var entities = _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .Where(c => c.Year == year);

            if (!entities.Any())
                return new ServiceResponse("Автомобілі не знайдено");

            var result = await CreatePagedResultAsync(pageNumber, pageSize, entities);
            return new ServiceResponse("Автомобілі отримано", true, result);
        }

        public async Task<ServiceResponse> GetPagedByManufactureAsync(int pageNumber, int pageSize, string? manufacture)
        {
            if (string.IsNullOrEmpty(manufacture))
                return new ServiceResponse("Виробника не вказано");

            var entities = _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .Where(c => c.Manufacture != null && c.Manufacture.Name == manufacture);

            if (!entities.Any())
                return new ServiceResponse("Автомобілі не знайдено");

            var result = await CreatePagedResultAsync(pageNumber, pageSize, entities);
            return new ServiceResponse("Автомобілі отримано", true, result);
        }

        public async Task<ServiceResponse> GetPagedByGearBoxAsync(int pageNumber, int pageSize, string? gearbox)
        {
            if (string.IsNullOrEmpty(gearbox))
                return new ServiceResponse("Коробку передач не вказано");

            var entities = _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .Where(c => c.Gearbox == gearbox);

            if (!entities.Any())
                return new ServiceResponse("Автомобілі не знайдено");

            var result = await CreatePagedResultAsync(pageNumber, pageSize, entities);
            return new ServiceResponse("Автомобілі отримано", true, result);
        }

        public async Task<ServiceResponse> GetPagedByColorAsync(int pageNumber, int pageSize, string? color)
        {
            if (string.IsNullOrEmpty(color))
                return new ServiceResponse("Коробку передач не вказано");

            var entities = _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .Where(c => c.Color == color);

            if (!entities.Any())
                return new ServiceResponse("Автомобілі не знайдено");

            var result = await CreatePagedResultAsync(pageNumber, pageSize, entities);
            return new ServiceResponse("Автомобілі отримано", true, result);
        }

        public async Task<ServiceResponse> GetPagedByModelAsync(int pageNumber, int pageSize, string? model)
        {
            if (string.IsNullOrEmpty(model))
                return new ServiceResponse("Коробку передач не вказано");

            var keywords = model.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var entities = _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .Where(c => keywords.Any(k => c.Model.Contains(k, StringComparison.OrdinalIgnoreCase)));

            if (!entities.Any())
                return new ServiceResponse("Автомобілі не знайдено");

            var result = await CreatePagedResultAsync(pageNumber, pageSize, entities);
            return new ServiceResponse("Автомобілі отримано", true, result);
        }
    }
}
