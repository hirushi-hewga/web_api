using Microsoft.AspNetCore.Identity;
using web_api.BLL.DTOs.Role;
using web_api.BLL.Services.Role;
using web_api.DAL.Entities;

namespace web_api.DataInitializer
{
    public class Seeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new AppRole{ Name = "admin" });
            
            if (await roleManager.FindByNameAsync("user") == null)
                await roleManager.CreateAsync(new AppRole{ Name = "user" });

            if (await roleManager.FindByNameAsync("carManager") == null)
                await roleManager.CreateAsync(new AppRole { Name = "carManager" });

            if (await roleManager.FindByNameAsync("manufactureManager") == null)
                await roleManager.CreateAsync(new AppRole { Name = "manufactureManager" });

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var user = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "admin",
                    LastName = "admin",
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(user, "qwerty-1");
                await userManager.AddToRoleAsync(user, "admin");
            }

            if (await userManager.FindByNameAsync("user") == null)
            {
                var user = new AppUser
                {
                    UserName = "user",
                    Email = "user@gmail.com",
                    FirstName = "user",
                    LastName = "user",
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(user, "qwerty-1");
                await userManager.AddToRoleAsync(user, "user");
            }
        }
    }
}