using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.OderDTO;
using Oto_Api.Application.Interfaces;

namespace Oto_Api.BackEnd.Controllers.USER.OrderManager
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderManager : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository  ;
                }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
        {
            if (dto == null || dto.OrderDetails == null || !dto.OrderDetails.Any())
                return BadRequest("Dữ liệu không hợp lệ.");

            var result = await _orderRepository.CreateOrderAsync(dto);

            if (result)
                return Ok(new { success = true, message = "Đặt hàng thành công" });
            else
                return StatusCode(500, new { success = false, message = "Đặt hàng thất bại" });
        }

    }
}
