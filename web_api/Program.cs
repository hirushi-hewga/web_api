using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using web_api.BLL;
using web_api.BLL.DTOs.Account;
using web_api.BLL.MapperProfiles;
using web_api.BLL.Services.Account;
using web_api.BLL.Services.Cars;
using web_api.BLL.Services.Email;
using web_api.BLL.Services.Image;
using web_api.BLL.Services.Jwt;
using web_api.BLL.Services.Manufactures;
using web_api.BLL.Services.Role;
using web_api.BLL.Services.User;
using web_api.DAL;
using web_api.DAL.Entities;
using web_api.DAL.Repositories.Cars;
using web_api.DAL.Repositories.Manufactures;
using web_api.DataInitializer;
using web_api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add Logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? "")),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IManufactureService, ManufactureService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Add repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();

builder.Services.AddControllers();

// Add fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Add automapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<RoleMapperProfile>();
    cfg.AddProfile<UserMapperProfile>();
    cfg.AddProfile<CarMapperProfile>();
    cfg.AddProfile<ManufactureMapperProfile>();
});

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("name=NpgSqlLocal");
});

// Add identity
builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("localhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header,
        Description = "Enter bearer JWT token"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new []{ Settings.AdminRole }
        }
    });
});

var app = builder.Build();

// Middlewares
app.UseMiddleware<MiddlewareLogger>();
app.UseMiddleware<MiddlewareExceptionHandler>();
app.UseMiddleware<MiddlewareNullExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Static files
var rootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
var imagesPath = Path.Combine(rootPath, Settings.ImagesPath);

if (!Directory.Exists(rootPath))
    Directory.CreateDirectory(rootPath);

if (!Directory.Exists(imagesPath))
    Directory.CreateDirectory(imagesPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images"
});

app.UseCors("localhost3000");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await Seeder.SeedAsync(services);
}

app.Run();
