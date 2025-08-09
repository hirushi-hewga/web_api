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

namespace web_api.BLL.Services.Manufactures
{
    public class ManufactureService : IManufactureService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ManufactureService(AppDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
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

            await _context.Manufactures.AddAsync(entity);
            var result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> UpdateAsync(ManufactureUpdateDto dto)
        {
            var entity = await _context.Manufactures
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

            _context.Manufactures.Update(entity);
            var result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Manufactures.FirstOrDefaultAsync(m => m.Id == id);

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.Image))
                    _imageService.DeleteImage(entity.Image);

                _context.Manufactures.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }

            return false;
        }

        public async Task<IEnumerable<ManufactureDto>> GetAllAsync()
        {
            var entities = await _context.Manufactures.ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ManufactureDto>>(entities);

            return dtos;
        }

        public async Task<ManufactureDto?> GetByIdAsync(string id)
        {
            var entity = await _context.Manufactures.FirstOrDefaultAsync(m => m.Id == id);

            if (entity == null)
                return null;

            var dto = _mapper.Map<ManufactureDto>(entity);

            return dto;
        }
    }
}
