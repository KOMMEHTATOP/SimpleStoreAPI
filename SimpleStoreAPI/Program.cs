using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

            //Кнопка авторизации в сваггере
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SimpleStore Api", Version = "v1"
                });
                //AddSecurityDefinition - Описание схемы безопасности
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,        // Где искать токен - в заголовке
                    Description = "Please enter a valid token", // Подсказка для пользователя
                    Name = "Authorization",               // Имя заголовка HTTP
                    Type = SecuritySchemeType.Http,       // Тип - HTTP аутентификация
                    BearerFormat = "JWT",                 // Формат токена
                    Scheme = "Bearer"                     // Схема - Bearer
                });
                
                //AddSecurityRequirement - Применение схемы
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer"  // Ссылка на схему выше
                            }
                        },
                        new string[] {}  // Области доступа (scopes) - у нас нет
                    }
                });
            });
            
            // Регистрация сервисов в DI контейнере
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenGenerator, TokenGeneratorService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("CanSellProducts", policy => policy.RequireRole("Admin", "Seller"))
                .AddPolicy("CanBuyProducts", policy => policy.RequireRole("Admin", "Seller", "Customer"))
                .AddPolicy("CanViewOwnOrders", policy => policy.RequireAuthenticatedUser())
                .AddPolicy("CanManageUsers", policy => policy.RequireRole("Admin"))
                .AddPolicy("CanAssignRoles", policy => policy.RequireRole("Admin"));

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
