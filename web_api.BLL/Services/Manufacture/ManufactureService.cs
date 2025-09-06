using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Manufactures;
using web_api.BLL.Services.Image;
using web_api.DAL;
using web_api.DAL.Entities;
using web_api.DAL.Repositories.Manufactures;

namespace web_api.BLL.Services.Manufactures
{
    public class ManufactureService : IManufactureService
    {
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ManufactureService(IManufactureRepository manufactureRepository,
            IImageService imageService,
            IMapper mapper)
        {
            _manufactureRepository = manufactureRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(ManufactureCreateDto dto)
        {
            var entity = _mapper.Map<Manufacture>(dto);

            if (dto.Image != null)
            {
                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesPath);
                if (imageName != null)
                {
                    imageName = Path.Combine(Settings.ManufacturesPath, imageName);
                }
                entity.Image = imageName;
            }

            var result = await _manufactureRepository.CreateAsync(entity);
            if (result)
                return new ServiceResponse($"Виробника '{dto.Name}' успішно створено", true);

            return new ServiceResponse("Не вдалося створити виробника");
        }

        public async Task<ServiceResponse> UpdateAsync(ManufactureUpdateDto dto)
        {
            var entity = await _manufactureRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == dto.Id);
            
            if (entity == null)
                return new ServiceResponse("Виробника не знайдено");

            entity = _mapper.Map(dto, entity);

            if (dto.Image != null)
            {
                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesPath);
                if (imageName != null)
                {
                    imageName = Path.Combine(Settings.ManufacturesPath, imageName);
                }

                if (!string.IsNullOrEmpty(entity.Image) && !string.IsNullOrEmpty(imageName))
                {
                    _imageService.DeleteImage(entity.Image);
                }

                entity.Image = imageName;
            }

            var result = await _manufactureRepository.UpdateAsync(entity);
            if (result)
                return new ServiceResponse($"Виробника {entity.Name} успішно оновлено", true);

            return new ServiceResponse("Не вдалося оновити виробника");
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);

            if (entity == null)
                return new ServiceResponse("Виробника не знайдено");

            if (!string.IsNullOrEmpty(entity.Image))
                _imageService.DeleteImage(entity.Image);

            var result = await _manufactureRepository.DeleteAsync(entity);
            if (result)
                return new ServiceResponse("Виробника видалено");

            return new ServiceResponse("Не вдалося видалити виробника");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _manufactureRepository.GetAll().ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ManufactureDto>>(entities);

            return new ServiceResponse("Виробників отримано", true, dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);

            if (entity == null)
                return new ServiceResponse("Виробника не знайдено");

            var dto = _mapper.Map<ManufactureDto>(entity);

            return new ServiceResponse("Виробника отримано", true, dto);
        }
    }
}
