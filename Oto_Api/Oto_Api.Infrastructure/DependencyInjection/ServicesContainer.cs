using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oto_Api.Application.DTOs.Auth;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Cart;
using Oto_Api.Infrastructure.Category;
using Oto_Api.Infrastructure.Data;
using Oto_Api.Infrastructure.Price;
using Oto_Api.Infrastructure.Product;
using Oto_Api.Infrastructure.Services;
using Oto_Api.Infrastructure.StatisticsOrder;
using Oto_Api.Infrastructure.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IEmailSender = Oto_Api.Application.Interfaces.IEmailSender;



namespace Oto_APIs.Infrastructure.DepnedencyInjection
{
    public static class ServicesContainer
    {

        public static IServiceCollection MainService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string connection = config.GetConnectionString("OtoDB");
                if (string.IsNullOrEmpty(connection))
                    throw new ArgumentNullException("Ket noi khong co.");
                options.UseSqlServer(connection);
            });

            // CORS Configuration
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()  // <== Cho phép tất cả origin
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            

            // JWT Configuration
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true, // Đảm bảo xác thực Audience nếu bạn sử dụng
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JWT:ValidIssuer"],  // Đọc từ cấu hình
                    ValidAudience = config["JWT:ValidAudience"],  // Đọc từ cấu hình
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]))
                };
            });
            

            // Identity Configuration
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            })
              .AddRoles<IdentityRole>()
              .AddRoleManager<RoleManager<IdentityRole>>()
              .AddUserManager<UserManager<User>>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddSignInManager<SignInManager<User>>()
              .AddDefaultTokenProviders();

            services.AddAuthentication();
            services.AddAuthorization();
            services.AddRazorPages();

            // Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //
            services.AddScoped<IAuthService, AuthService>();
            // Cấu hình JSON để dùng ReferenceHandler.Preserve
            //services.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //        options.JsonSerializerOptions.WriteIndented = true; // Cho đẹp (tùy chọn)
            //    });
            // Đăng ký EmailSender
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderStatisticsRepository, OrderStatisticsRepository>();
            return services;

        }
    }
}
