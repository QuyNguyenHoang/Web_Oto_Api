using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oto_Api.Application.DTOs.Auth;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;
using Oto_Api.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



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
                options.AddPolicy("client_cors", builder =>
                {
                    builder.WithOrigins("http://127.0.0.1:5501")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
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

            return services;

        }
    }
}
