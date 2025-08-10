using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories.Cars
{
    public interface ICarRepository
        : IGenericRepository<Car, string>
    {
        Task<bool> CreateAsync(Car entity);
        Task<bool> UpdateAsync(Car entity);
        Task<bool> DeleteAsync(Car entity);
        Task<Car?> GetById(string id);
        IQueryable<Car> GetAll();
    }
}
