using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Oto_Api.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Oto_Api.Application.DTOs.Auth;
using Oto_Api.Application.Interfaces;
using Oto_Api.Application.DTOs;
using System.Web;
using System.Text.Encodings.Web;

namespace Oto_Api.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly Application.Interfaces.IEmailSender _emailSender;
        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IConfiguration configuration, RoleManager<IdentityRole> roleManager, Application.Interfaces.IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.roleManager = roleManager;
            _emailSender = emailSender;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
                return "Email đã được sử dụng.";

            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return string.Join("; ", result.Errors.Select(e => e.Description));

            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            }

            await _userManager.AddToRoleAsync(user, Roles.User);

            // Gửi email xác thực
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var confirmLink = $"{_configuration["FrontendUrl"]}/Source/confirm.html?userId={user.Id}&token={encodedToken}";
            var safeConfirmLink = HtmlEncoder.Default.Encode(confirmLink);
            var emailBody = $@"
    <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f9;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    padding: 30px;
                }}
                .header {{
                    text-align: center;
                    color: #333;
                    margin-bottom: 20px;
                }}
                .header h2 {{
                    font-size: 24px;
                    color: #4CAF50;
                }}
                .content {{
                    font-size: 16px;
                    line-height: 1.5;
                    margin-bottom: 20px;
                }}
                .content a {{
                    font-size: 18px;
                    color: #ffffff;
                    background-color: #4CAF50;
                    padding: 12px 20px;
                    text-decoration: none;
                    border-radius: 4px;
                    display: inline-block;
                    font-weight: bold;
                }}
                .content a:hover {{
                    background-color: #45a049;
                }}
                .footer {{
                    text-align: center;
                    font-size: 14px;
                    color: #888;
                    margin-top: 30px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h2>Xác thực tài khoản của bạn</h2>
                </div>
                <div class='content'>
                    <p>Chào bạn,</p>
                    <p>Để hoàn tất việc đăng ký tài khoản, vui lòng nhấn vào liên kết bên dưới để xác thực email của bạn:</p>
                    <p><a href='{safeConfirmLink}'>Xác thực email</a></p>
                    <p>Trân trọng,<br>Đội ngũ hỗ trợ</p>
                </div>
                <div class='footer'>
                    <p>Nếu bạn không yêu cầu đăng ký này, vui lòng bỏ qua email này.</p>
                </div>
            </div>
        </body>
    </html>";



            await _emailSender.SendEmailAsync(user.Email, "Xác thực email", emailBody);

            return "Đăng ký thành công. Vui lòng kiểm tra email để xác thực.";
        }


        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return "Tài khoản hoặc mật khẩu sai.";

            if (!user.EmailConfirmed)
                return "Vui lòng xác thực Email để đăng nhập";

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return "Tài khoản hoặc mật khẩu sai.";

            // Tạo claims cho JWT
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id), // 👈 Thêm UserId
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
