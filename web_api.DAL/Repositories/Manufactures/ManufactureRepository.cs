using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories.Manufactures
{
    public class ManufactureRepository
        : GenericRepository<Manufacture, string>,
        IManufactureRepository
    {
        public ManufactureRepository(AppDbContext context)
            : base(context){}
    }
}
