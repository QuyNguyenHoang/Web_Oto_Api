using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Oto_Api.BackEnd.Controllers.UserManager
{
    public class UserManager : Controller
    {
        [Authorize(Roles ="admin")]
        [HttpGet("GetUserData")]
        public IActionResult GetUserData()
        {
            return Ok("Bạn đã xác thực thành công!");
        }

    }
}
