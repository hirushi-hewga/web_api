using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_api.DAL.Entities;

namespace web_api.DAL
{
    public class AppDbContext(DbContextOptions options) 
        : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Car> Cars { get; set; }
    }
}
