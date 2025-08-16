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
        Task<ServiceResponse> CreateAsync(CarCreateDto dto);
        Task<ServiceResponse> UpdateAsync(CarUpdateDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetPagedAsync(int pageNumber, int pageSize);
        Task<ServiceResponse> GetPagedByYearAsync(int pageNumber, int pageSize, int? year);
        Task<ServiceResponse> GetPagedByManufactureAsync(int pageNumber, int pageSize, string? manufacture);
        Task<ServiceResponse> GetPagedByGearBoxAsync(int pageNumber, int pageSize, string? gearbox);
        Task<ServiceResponse> GetPagedByColorAsync(int pageNumber, int pageSize, string? color);
        Task<ServiceResponse> GetPagedByModelAsync(int pageNumber, int pageSize, string? model);
    }
}
