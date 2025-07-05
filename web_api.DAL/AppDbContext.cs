using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_api.DAL.Entities;

namespace web_api.DAL
{
    public class AppDbContext
        : IdentityDbContext<AppUser, AppRole, string, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public AppDbContext(DbContextOptions options) : base(options) {}
        
        public DbSet<Car> Cars { get; set; }
    }
}
