using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.BLL.DTOs.Cars;

namespace web_api.BLL.Services.Cars
{
    public interface ICarService
    {
        Task<bool> CreateAsync(CarCreateDto dto);
        Task<bool> UpdateAsync(CarUpdateDto dto);
        Task<bool> DeleteAsync(string id);
        Task<CarDto?> GetByIdAsync(string id);
        Task<IEnumerable<CarDto>> GetAllAsync();
    }
}
