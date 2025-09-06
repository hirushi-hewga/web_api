using AutoMapper;
using Microsoft.EntityFrameworkCore;
using web_api.BLL.DTOs;
using web_api.BLL.DTOs.Cars;
using web_api.BLL.Services.Image;
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

        public async Task<ServiceResponse> GetAllAsync(int? year, string? manufacture, string? gearbox, string? color, string? model, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return new ServiceResponse("Invalid pageNumber or pageSize.");

            var entities = _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .AsQueryable();

            if (year != null)
                entities = entities.Where(c => c.Year == year);

            if (!string.IsNullOrEmpty(manufacture))
                entities = entities.Where(c => c.Manufacture != null && c.Manufacture.Name.Contains(manufacture, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(gearbox))
                entities = entities.Where(c => c.Gearbox != null && c.Gearbox.Contains(gearbox, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(color))
                entities = entities.Where(c => c.Color != null && c.Color.Contains(color, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(model))
            {
                var keywords = model.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                entities = entities.Where(c => keywords.Any(k => c.Model.Contains(k, StringComparison.OrdinalIgnoreCase)));
            }

            var result = await CreatePagedResultAsync(pageNumber, pageSize, entities);
            return new ServiceResponse(result.Items.Any() ? "Автомобілі отримано" : "Автомобілі не знайдено", true, result);
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

            pageNumber = pageNumber < 1 || pageNumber > totalPages ? 1 : pageNumber;

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
    }
}
