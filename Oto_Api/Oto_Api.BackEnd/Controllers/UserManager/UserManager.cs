using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.UserDTO;
using Oto_Api.Application.Interfaces;
using System.Threading.Tasks;

namespace Oto_Api.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserManagerController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetUserDtoByIdAsync(id);
            if (user == null)
                return NotFound();


            return Ok(user);
        }
        [HttpPost("Create_UserInf")]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] UserDto userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userRepository.GetUserDtoByIdAsync(userRequest.Id);

            var result = await _userRepository.CreateOrUpdateUserAsync(userRequest);

            if (existingUser == null)
            {
                // Nếu không có trước đó -> là tạo mới
                return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
            }
            else
            {
                // Nếu đã có -> là cập nhật
                return Ok(result);
            }
        }

    }
}
