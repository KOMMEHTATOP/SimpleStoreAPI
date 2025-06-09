using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleStoreAPI.Data;
using SimpleStoreAPI.Interfaces;
using SimpleStoreAPI.Interfaces.Auth;
using SimpleStoreAPI.Models;
using SimpleStoreAPI.Service;
using System.Text;

namespace SimpleStoreAPI
{
   public class Program
   {
       public static void Main(string[] args)
       {
           var builder = WebApplication.CreateBuilder(args);

           builder.Services.AddControllers();

           // Регистрация Entity Framework
           builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
           
           // Регистрация ASP.NET Identity
           builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();
           
           // Настройка аутентификации с использованием JWT токенов
           builder.Services.AddAuthentication(options =>
           {
               // Установка схемы аутентификации по умолчанию
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           })
               .AddJwtBearer(options =>
               {
                   // Получение секретного ключа из конфигурации
                   var jwtKey = builder.Configuration["Jwt:Key"];
                   var keyInBytes = Encoding.UTF8.GetBytes(jwtKey);
                   var securityKey = new SymmetricSecurityKey(keyInBytes);
                   
                   // Настройка параметров валидации JWT токенов
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["Jwt:Issuer"],
                       ValidAudience = builder.Configuration["Jwt:Audience"],
                       IssuerSigningKey = securityKey
                   };
               });

           builder.Services.AddEndpointsApiExplorer();
           builder.Services.AddSwaggerGen();
           
           // Регистрация сервисов в DI контейнере
           builder.Services.AddScoped<IAuthService, AuthService>();
           builder.Services.AddScoped<ITokenGenerator, TokenGeneratorService>();
           builder.Services.AddScoped<IRoleService, RoleService>();

           var app = builder.Build();

           if (app.Environment.IsDevelopment())
           {
               app.UseSwagger();
               app.UseSwaggerUI();
           }
           
           app.UseHttpsRedirection();
           app.UseAuthentication();
           app.UseAuthorization();
           app.MapControllers();

           app.Run();
       }
   }
}