using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.Auth;
using System.Threading.Tasks;

namespace Oto_Api.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var result = await _authService.RegisterAsync(registerDto);
            return result == "Đăng ký thành công" ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var result = await _authService.LoginAsync(loginDto);
            return result.StartsWith("Tài khoản hoặc mật khẩu sai") ? Unauthorized(result) : Ok(new {message ="Success", Token = result });
        }
    }
}
