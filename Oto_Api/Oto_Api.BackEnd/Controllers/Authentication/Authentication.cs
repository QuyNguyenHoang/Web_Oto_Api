using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.Auth;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Oto_Api.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthService authService, UserManager<User>  userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var result = await _authService.RegisterAsync(registerDto);
            return result == "Đăng ký thành công. Vui lòng kiểm tra email để xác thực." ? Ok(new {success= true, message = result }) : BadRequest(new {error = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var result = await _authService.LoginAsync(loginDto);
            return result.StartsWith("Tài khoản hoặc mật khẩu sai")||result.StartsWith("Vui lòng xác thực Email để đăng nhập") ? Unauthorized(result) : Ok(new {message ="Success", Token = result });
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Thiếu userId hoặc token");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Không tìm thấy người dùng");

            //var decodedToken = WebUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return Ok("Email đã được xác thực thành công!");

            var errors = string.Join(", ", result.Errors.Select(e => e.Description)); // Lấy chi tiết lỗi
            return BadRequest($"Xác thực email thất bại: {errors}");
        }

    }
}
