using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Manufactures;
using web_api.DAL;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Manufactures
{
    public class ManufactureService : IManufactureService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ManufactureService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ManufactureDto dto)
        {
            if (await _context.Manufactures.AnyAsync(m => m.Name.ToUpper() == dto.Name!.ToUpper()))
                return false;

            var entity = _mapper.Map<Manufacture>(dto);

            await _context.Manufactures.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(ManufactureDto dto)
        {
            if (await _context.Manufactures.Where(m => m.Id != dto.Id)
                .FirstOrDefaultAsync(m => m.Name.ToUpper() == dto.Name!.ToUpper()) != null)
                return false;

            var entity = _mapper.Map<Manufacture>(dto);

            _context.Manufactures.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Manufactures.FirstOrDefaultAsync(m => m.Id == id);

            if (entity != null)
            {
                _context.Manufactures.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
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
