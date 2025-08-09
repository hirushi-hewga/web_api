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

namespace web_api.BLL.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CarService(AppDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<bool> CreateAsync(CarCreateDto dto)
        {
            var entity = _mapper.Map<Car>(dto);

            if (dto.Images != null && dto.Images.Any())
            {
                entity.Images = (await Task.WhenAll(
                    dto.Images.Select(i => _imageService.SaveImageAsync(i, Settings.ManufacturesPath))
                    )).Where(name => !string.IsNullOrEmpty(name))
                    .Select(name => Path.Combine(Settings.ManufacturesPath, name));
            }

            await _context.Cars.AddAsync(entity);
            var result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> UpdateAsync(CarUpdateDto dto)
        {
            var entity = await _context.Cars
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (entity == null)
            {
                return false;
            }

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

            _context.Cars.Update(entity);
            var result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);

            if (entity != null)
            {
                if (entity.Images != null && entity.Images.Any())
                {
                    entity.Images.ToList().ForEach(_imageService.DeleteImage);
                }

                _context.Cars.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }

            return false;
        }

        public async Task<IEnumerable<CarDto>> GetAllAsync()
        {
            var entities = await _context.Cars.ToListAsync();

            var dtos = _mapper.Map<IEnumerable<CarDto>>(entities);

            return dtos;
        }

        public async Task<CarDto?> GetByIdAsync(string id)
        {
            var entity = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null)
                return null;

            var dto = _mapper.Map<CarDto>(entity);

            return dto;
        }
    }
}
