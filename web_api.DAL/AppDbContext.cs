using Microsoft.EntityFrameworkCore;

namespace web_api.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
