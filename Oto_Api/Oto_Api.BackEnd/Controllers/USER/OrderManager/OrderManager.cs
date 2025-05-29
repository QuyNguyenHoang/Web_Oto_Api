using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("GetOrderByUser/{id}")]
        public async Task<IActionResult> GetOrdersByUserId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Thiếu userId");
            }

            var orders = await _orderRepository.GetOrdersByUserIdAsync(id);

            if (orders == null || !orders.Any())
            {
                return NotFound("Không tìm thấy đơn hàng nào cho người dùng này.");
            }

            return Ok(orders); 
        }
        [HttpGet("GetOrderDetail/{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrderDetail(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);

            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng." });

            return Ok(order);
        }


    }
}
