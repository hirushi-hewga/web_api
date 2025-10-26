using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories.Cars
{
    public class CarRepository
        : GenericRepository<Car, string>,
        ICarRepository
    {
        public CarRepository(AppDbContext context)
            : base(context) { }

        public IQueryable<Car> GetCars(List<Expression<Func<Car, bool>>>? preds = null)
        {
            var entities = GetAll()
                .Include(c => c.Manufacture)
                .Include(c => c.Images)
                .AsQueryable();

            if (preds != null)
            {
                foreach (var pred in preds)
                {
                    entities = entities.Where(pred);
                }
            }

            return entities;
        }
    }
}
