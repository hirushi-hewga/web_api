using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories.Manufactures
{
    public interface IManufactureRepository
        : IGenericRepository<Manufacture, string>
    {
        Task<bool> CreateAsync(Manufacture entity);
        Task<bool> UpdateAsync(Manufacture entity);
        Task<bool> DeleteAsync(Manufacture entity);
        Task<Manufacture?> GetById(string id);
        IQueryable<Manufacture> GetAll();
    }
}
