using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Cars;
using web_api.BLL.DTOs.Manufactures;
using web_api.BLL.Services.Image;
using web_api.DAL;
using web_api.DAL.Entities;
using web_api.DAL.Repositories.Cars;
using web_api.DAL.Repositories.Manufactures;

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

            return new ServiceResponse("Автомобіль не створено");
        }

        public async Task<ServiceResponse> UpdateAsync(CarUpdateDto dto)
        {
            var entity = await _carRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (entity == null)
                return new ServiceResponse("Автомобіль не знайдено");

            entity = _mapper.Map(dto, entity);

            if (dto.Images != null && dto.Images.Any())
            {
                if (entity.Images != null && entity.Images.Any())
                {
                    entity.Images.ToList().ForEach(_imageService.DeleteImage);
                }

                var imageNames = (await Task.WhenAll(
                    dto.Images.Select(i => _imageService.SaveImageAsync(i, Settings.ManufacturesPath))
                    )).Where(name => !string.IsNullOrEmpty(name))
                    .Select(name => Path.Combine(Settings.ManufacturesPath, name));

                entity.Images = imageNames;
            }

            var result = await _carRepository.UpdateAsync(entity);
            if (result)
                return new ServiceResponse($"Автомобіль '{entity.Brand} {entity.Model}' успішно оновлено", true);

            return new ServiceResponse("Автомобіль не оновлено");
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _carRepository.GetByIdAsync(id);

            if (entity == null)
                return new ServiceResponse("Автомобіль не знайдено");

            if (entity.Images != null && entity.Images.Any())
            {
                entity.Images.ToList().ForEach(_imageService.DeleteImage);
            }

            var result = await _carRepository.DeleteAsync(entity);
            if (result)
                return new ServiceResponse("Автомобіль видалено", true);

            return new ServiceResponse("Автомобіль не видалено");
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
    }
}
