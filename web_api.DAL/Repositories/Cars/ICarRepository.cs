using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories.Cars
{
    public interface ICarRepository
        : IGenericRepository<Car, string>
    {
        IQueryable<Car> GetCars(List<Expression<Func<Car, bool>>>? preds = null);
    }
}
