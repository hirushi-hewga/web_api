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
        Task<ServiceResponse> CreateAsync(ManufactureCreateDto dto);
        Task<ServiceResponse> UpdateAsync(ManufactureUpdateDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetAllAsync();
    }
}
