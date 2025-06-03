using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.OderDTO;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;

namespace Oto_Api.BackEnd.Controllers.OrderManager
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrderManager : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("Search_Order")]
        public async Task<IActionResult> Search(string searchTerm = "", int pageSize = 10, int pageNumber = 1, bool? statusFilter = null)
        {
            var orderData = await _orderRepository.SearchOrderAsync(searchTerm, pageSize, pageNumber, statusFilter);
            var orderCount = await _orderRepository.CountOrderAsync(searchTerm, statusFilter);

            if (orderData == null || !orderData.Any())
            {
                return NotFound(new { message = "No orders found" });
            }

            var totalPages = (int)Math.Ceiling((double)orderCount / pageSize);

            return Ok(new
            {
                data = orderData,
                currentPage = pageNumber,
                pageSize = pageSize,
                totalRecords = orderCount,
                totalPages = totalPages,
            });
        }
        [HttpPut("Update_Order/{id}")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDto dto, int id)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu đơn hàng không hợp lệ.");
            }

            var result = await _orderRepository.UpdateOrderAsync(dto, id);

            if (result)
            {
                return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công." });
            }
            else
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng cần cập nhật." });
            }
        }
        [HttpGet("Get_OrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = $"Không tìm thấy đơn hàng có ID = {id}" });
            }

            return Ok(order);
        }
        [HttpDelete("Delete_Order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.DeleteOrderAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng." });
            }

            return Ok(new { message = "Xóa đơn hàng thành công.", orderId = id });
        }

    }
}
