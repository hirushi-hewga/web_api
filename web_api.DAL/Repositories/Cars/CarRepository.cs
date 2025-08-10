using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
