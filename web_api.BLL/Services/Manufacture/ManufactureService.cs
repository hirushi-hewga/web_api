using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CreateAsync(ManufactureCreateDto dto)
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
            return result;
        }

        public async Task<bool> UpdateAsync(ManufactureUpdateDto dto)
        {
            var entity = await _manufactureRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == dto.Id);
            
            if (entity == null)
            {
                return false;
            }

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
            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.Image))
                    _imageService.DeleteImage(entity.Image);

                var result = await _manufactureRepository.DeleteAsync(entity);
                return result;
            }

            return false;
        }

        public async Task<IEnumerable<ManufactureDto>> GetAllAsync()
        {
            var entities = await _manufactureRepository.GetAll().ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ManufactureDto>>(entities);

            return dtos;
        }

        public async Task<ManufactureDto?> GetByIdAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            var dto = _mapper.Map<ManufactureDto>(entity);

            return dto;
        }
    }
}
