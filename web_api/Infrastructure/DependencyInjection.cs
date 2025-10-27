using Quartz;
using web_api.BLL.Services.Account;
using web_api.BLL.Services.Cars;
using web_api.BLL.Services.Email;
using web_api.BLL.Services.Image;
using web_api.BLL.Services.Jwt;
using web_api.BLL.Services.Manufactures;
using web_api.BLL.Services.Role;
using web_api.BLL.Services.User;
using web_api.Jobs;

namespace web_api.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IManufactureService, ManufactureService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IJwtService, JwtService>();
        }

        public static void AddJobs(this IServiceCollection services, params (Type type, string cronExpression)[] jobs)
        {
            services.AddQuartz(q =>
            {
                foreach (var job in jobs)
                {
                    var jobKey = new JobKey(job.type.Name);
                    q.AddJob(job.type, jobKey, opt => { });

                    q.AddTrigger(opt => opt
                        .ForJob(jobKey)
                        .WithIdentity($"{job.type.Name}-trigger")
                        .WithCronSchedule(job.cronExpression));
                }
            });
        }
    }
}
