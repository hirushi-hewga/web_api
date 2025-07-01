using Microsoft.EntityFrameworkCore;
using web_api.DAL.Entities;

namespace web_api.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Car> Cars { get; set; }
    }
}
