using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Manufactures;

namespace web_api.BLL.Services.Manufactures
{
    public interface IManufactureService
    {
        Task<bool> CreateAsync(ManufactureDto dto);
        Task<bool> UpdateAsync(ManufactureDto dto);
        Task<bool> DeleteAsync(string id);
        Task<ManufactureDto?> GetByIdAsync(string id);
        Task<IEnumerable<ManufactureDto>> GetAllAsync();
    }
}
