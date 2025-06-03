using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.OrderStatisticsDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Infrastructure.StatisticsOrder;
using System.Globalization;

namespace Oto_Api.BackEnd.Controllers.OrderStatisticsManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatisticsManager : Controller
    {
        private readonly IOrderStatisticsRepository _orderStatisticsRepo;
        public OrderStatisticsManager(IOrderStatisticsRepository orderStatisticsRepo)
        {
            _orderStatisticsRepo = orderStatisticsRepo;
        }

        [HttpPost("generate-daily")]
        public async Task<IActionResult> GenerateDailyStatistic([FromQuery] DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;
            var result = await _orderStatisticsRepo.CreateOrUpdateOrderStatisticsAsync(targetDate);

            if (result)
                return Ok(new { success = true, message = $"Đã cập nhật thống kê ngày {targetDate:yyyy-MM-dd}" });
            else
                return NotFound(new { success = false, message = "Không có đơn hàng nào để thống kê." });
        }



        [HttpGet("CreateByMonth")]
        public async Task<IActionResult> CreateByMonth([FromQuery] string month)
        {
            if (string.IsNullOrEmpty(month) || !DateTime.TryParse(month + "-01", out var parsedDate))
            {
                return BadRequest(new { success = false, message = "Tháng không hợp lệ." });
            }

            int year = parsedDate.Year;
            int mon = parsedDate.Month;

            var result = await _orderStatisticsRepo.CreateOrUpdateOrderStatisticsByMonthAsync(year, mon);

            if (result)
                return Ok(new { success = true, message = $"Đã cập nhật thống kê tháng {month}" });
            else
                return NotFound(new { success = false, message = "Không có đơn hàng nào trong tháng để thống kê." });
        }


        [HttpGet("CreateByYear")]
        public async Task<IActionResult> CreateByYear([FromQuery] int year)
        {
            if (year < 2000 || year > DateTime.Today.Year)
            {
                return BadRequest(new { success = false, message = "Năm không hợp lệ." });
            }

            var result = await _orderStatisticsRepo.CreateOrUpdateOrderStatisticsByYearAsync(year);

            if (result)
                return Ok(new { success = true, message = $"Đã cập nhật thống kê năm {year}" });
            else
                return NotFound(new { success = false, message = "Không có đơn hàng nào trong năm để thống kê." });
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderStatisticsDto>>> GetAllOrderStatistics()
        {
            var statistics = await _orderStatisticsRepo.GetAllOrderStatisticsAsync();

            if (statistics == null || statistics.Count == 0)
                return NotFound("Chưa có dữ liệu thống kê.");

            return Ok(statistics);
        }
        [HttpGet("Search_OrderStatistics")]
        public async Task<IActionResult> Search(DateTime? searchDate, int pageSize = 10, int pageNumber = 1)
        {
            var orderData = await _orderStatisticsRepo.SearchOrderStatisticsByDateAsync(searchDate, pageNumber, pageSize);
            var orderCount = await _orderStatisticsRepo.CountOrderStatisticsAsync(searchDate);

            if (orderData == null || !orderData.Any())
            {
                return NotFound(new { message = "No orders found" });
            }

            int totalPages = (int)Math.Ceiling((double)orderCount / pageSize);

            return Ok(new
            {
                data = orderData,
                currentPage = pageNumber,
                pageSize = pageSize,
                totalRecords = orderCount,
                totalPages = totalPages,
            });
        }
        [HttpDelete("Delete_OrderStatistics/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _orderStatisticsRepo.DeleteOrderStatisticsAsync(id);
                if (!result)
                    return NotFound($"Không tìm thấy thống kê với ID = {id}");

                return Ok($"Đã xóa thống kê với ID = {id} thành công.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Lỗi máy chủ, vui lòng thử lại sau.");
            }
        }
        [HttpGet("day")]
        public async Task<IActionResult> GetDayStatistics([FromQuery] DateTime? date)
        {
            var result = await _orderStatisticsRepo.GetDayOrderStatisticsAsync(date);
            if (result == null)
                return NotFound("Không tìm thấy thống kê cho ngày này.");
            return Ok(result);
        }
        [HttpGet("month")]
        public async Task<IActionResult> GetMonthStatistics([FromQuery] int? month, [FromQuery] int? year)
        {
            var result = await _orderStatisticsRepo.GetMonthOrderStatisticsAsync(month, year);
            if (result == null)
                return NotFound("Không tìm thấy thống kê cho tháng này.");
            return Ok(result);
        }
        [HttpGet("year")]
        public async Task<IActionResult> GetYearStatistics([FromQuery] int? year)
        {
            var result = await _orderStatisticsRepo.GetYearOrderStatisticsAsync(year);
            if (result == null)
                return NotFound("Không tìm thấy thống kê cho năm này.");
            return Ok(result);
        }
    }


}




