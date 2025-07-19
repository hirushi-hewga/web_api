using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using web_api.BLL.DTOs.Role;
using web_api.DAL.Entities;

namespace web_api.BLL.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public async Task<bool> CreateAsync(RoleDto dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.Name))
                return false;

            var entity = new AppRole
            {
                Id = dto.Id,
                Name = dto.Name
            };
            
            var result = await _roleManager.CreateAsync(entity);
            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(RoleDto dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.Name))
                return false;

            var entity = new AppRole
            {
                Id = dto.Id,
                Name = dto.Name
            };
            
            var result = await _roleManager.UpdateAsync(entity);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);
            
            if (entity != null)
            {
                var result = await _roleManager.DeleteAsync(entity);
                return result.Succeeded;
            }
            
            return false;
        }

        public async Task<RoleDto?> GetByIdAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity == null)
                return null;
            
            var dto = new RoleDto
            { 
                Id = entity.Id, 
                Name = entity.Name ?? string.Empty
            };
            
            return dto;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var entities = await _roleManager.Roles.ToListAsync();

            var dtos = entities
                .Select(e => new RoleDto { Id = e.Id, Name = e.Name ?? "" });
            
            return dtos;
        }
    }
}