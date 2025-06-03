using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.OrderStatisticsDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Oto_Api.Infrastructure.StatisticsOrder
{
    public class OrderStatisticsRepository : IOrderStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderStatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOrUpdateOrderStatisticsAsync(DateTime date)
        {
            var targetDate = date.Date;

            var ordersToday = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Date == targetDate)
                .ToListAsync();

            if (!ordersToday.Any())
                return false; // Không có đơn hàng nào hôm nay

            var totalOrders = ordersToday.Count;
            var totalRevenue = ordersToday.Sum(o => o.TotalAmount);
            var totalPaidOrders = ordersToday.Count(o => o.Status == true);
            var totalUnpaidOrders = ordersToday.Count(o => o.Status == false);
            var totalCustomers = ordersToday.Select(o => o.Id).Distinct().Count();

            // Tổng sản phẩm bán ra (kiểm tra null trước khi tính)
            var totalProductsSold = ordersToday.Sum(o =>
                o.OrderDetails != null
                ? o.OrderDetails.Sum(od => od.Quantity)
                : 0
            );

            // Kiểm tra đã có thống kê ngày hôm nay chưa
            var statistic = await _context.OrderStatistics
                .FirstOrDefaultAsync(s => s.StatisticDate.Date == targetDate && s.StatisticType == "Day");

            if (statistic != null)
            {
                // Cập nhật nếu đã tồn tại
                statistic.TotalOrders = totalOrders;
                statistic.TotalRevenue = (decimal)totalRevenue;
                statistic.TotalPaidOrders = totalPaidOrders;
                statistic.TotalUnpaidOrders = totalUnpaidOrders;
                statistic.TotalCustomers = totalCustomers;
                statistic.TotalProductsSold = totalProductsSold;
            }
            else
            {
                // Tạo mới nếu chưa có
                statistic = new OrderStatistics
                {
                    StatisticDate = targetDate,
                    StatisticType = "Day",
                    TotalOrders = totalOrders,
                    TotalRevenue = (decimal)totalRevenue,
                    TotalPaidOrders = totalPaidOrders,
                    TotalUnpaidOrders = totalUnpaidOrders,
                    TotalCustomers = totalCustomers,
                    TotalProductsSold = totalProductsSold
                };

                await _context.OrderStatistics.AddAsync(statistic);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CreateOrUpdateOrderStatisticsByMonthAsync(int year, int month)
        {
            // Lấy ngày đầu và cuối của tháng
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Lấy các đơn hàng trong tháng, Include OrderDetails để tránh null
            var ordersInMonth = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Date >= firstDayOfMonth && o.OrderDate.Date <= lastDayOfMonth)
                .ToListAsync();

            if (!ordersInMonth.Any())
                return false; // Không có đơn hàng nào trong tháng

            var totalOrders = ordersInMonth.Count;
            var totalRevenue = ordersInMonth.Sum(o => o.TotalAmount);
            var totalPaidOrders = ordersInMonth.Count(o => o.Status == true);
            var totalUnpaidOrders = ordersInMonth.Count(o => o.Status == false);
            var totalCustomers = ordersInMonth.Select(o => o.Id).Distinct().Count();

            var totalProductsSold = ordersInMonth.Sum(o =>
                o.OrderDetails != null
                ? o.OrderDetails.Sum(od => od.Quantity)
                : 0
            );

            // Kiểm tra đã có thống kê tháng đó chưa
            var statistic = await _context.OrderStatistics
                .FirstOrDefaultAsync(s => s.StatisticDate.Year == year && s.StatisticDate.Month == month && s.StatisticType == "Month");

            if (statistic != null)
            {
                // Cập nhật nếu đã tồn tại
                statistic.TotalOrders = totalOrders;
                statistic.TotalRevenue = (decimal)totalRevenue;
                statistic.TotalPaidOrders = totalPaidOrders;
                statistic.TotalUnpaidOrders = totalUnpaidOrders;
                statistic.TotalCustomers = totalCustomers;
                statistic.TotalProductsSold = totalProductsSold;
            }
            else
            {
                // Tạo mới nếu chưa có
                statistic = new OrderStatistics
                {
                    StatisticDate = firstDayOfMonth, // dùng ngày đầu tháng làm đại diện
                    StatisticType = "Month",
                    TotalOrders = totalOrders,
                    TotalRevenue = (decimal)totalRevenue,
                    TotalPaidOrders = totalPaidOrders,
                    TotalUnpaidOrders = totalUnpaidOrders,
                    TotalCustomers = totalCustomers,
                    TotalProductsSold = totalProductsSold
                };

                await _context.OrderStatistics.AddAsync(statistic);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CreateOrUpdateOrderStatisticsByYearAsync(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);

            var ordersInYear = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
                .ToListAsync();

            if (!ordersInYear.Any())
                return false; // Không có đơn hàng trong năm

            var totalOrders = ordersInYear.Count;
            var totalRevenue = ordersInYear.Sum(o => o.TotalAmount);
            var totalPaidOrders = ordersInYear.Count(o => o.Status == true);
            var totalUnpaidOrders = ordersInYear.Count(o => o.Status == false);
            var totalCustomers = ordersInYear.Select(o => o.Id).Distinct().Count();

            var totalProductsSold = ordersInYear.Sum(o =>
                o.OrderDetails != null
                ? o.OrderDetails.Sum(od => od.Quantity)
                : 0
            );

            var statistic = await _context.OrderStatistics
                .FirstOrDefaultAsync(s => s.StatisticDate.Year == year && s.StatisticType == "Year");

            if (statistic != null)
            {
                statistic.TotalOrders = totalOrders;
                statistic.TotalRevenue = (decimal)totalRevenue;
                statistic.TotalPaidOrders = totalPaidOrders;
                statistic.TotalUnpaidOrders = totalUnpaidOrders;
                statistic.TotalCustomers = totalCustomers;
                statistic.TotalProductsSold = totalProductsSold;
            }
            else
            {
                statistic = new OrderStatistics
                {
                    StatisticDate = startDate,
                    StatisticType = "Year",
                    TotalOrders = totalOrders,
                    TotalRevenue = (decimal)totalRevenue,
                    TotalPaidOrders = totalPaidOrders,
                    TotalUnpaidOrders = totalUnpaidOrders,
                    TotalCustomers = totalCustomers,
                    TotalProductsSold = totalProductsSold
                };

                await _context.OrderStatistics.AddAsync(statistic);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<OrderStatisticsDto>> GetAllOrderStatisticsAsync()
        {
            var statistics = await _context.OrderStatistics
                .OrderByDescending(s => s.StatisticDate) // có thể sắp xếp theo ngày giảm dần
                .Select(s => new OrderStatisticsDto
                {
                    OrderStatisticId = s.OrderStatisticId,
                    StatisticDate = s.StatisticDate.ToString(s.StatisticType == "Day" ? "yyyy-MM-dd" :
                                                              s.StatisticType == "Month" ? "yyyy-MM" : "yyyy"),
                    TotalRevenue = s.TotalRevenue,
                    TotalOrders = s.TotalOrders,
                    TotalProductsSold = s.TotalProductsSold,
                    TotalPaidOrders = s.TotalPaidOrders,
                    TotalUnpaidOrders = s.TotalUnpaidOrders,
                    TotalCustomers = s.TotalCustomers,
                    StatisticType = s.StatisticType
                })
                .ToListAsync();

            return statistics;
        }
        public async Task<int> CountOrderStatisticsAsync(DateTime? searchDate)
        {
            var query = _context.OrderStatistics.AsQueryable();

            if (searchDate.HasValue)
            {
                var date = searchDate.Value.Date;
                query = query.Where(c => c.StatisticDate.Date == date);
            }

            return await query.CountAsync();
        }

        public async Task<List<OrderStatisticsDto>> SearchOrderStatisticsByDateAsync(DateTime? searchDate, int pageNumber, int pageSize)
        {
            var query = _context.OrderStatistics.AsQueryable();

            if (searchDate.HasValue)
            {
                var date = searchDate.Value.Date;
                query = query.Where(o => o.StatisticDate.Date == date);
            }

            var result = await query
                .OrderByDescending(o => o.StatisticDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderStatisticsDto
                {
                    OrderStatisticId = o.OrderStatisticId,
                    StatisticDate = o.StatisticDate.ToString("yyyy-MM-dd"),
                    TotalRevenue = o.TotalRevenue,
                    TotalOrders = o.TotalOrders,
                    TotalProductsSold = o.TotalProductsSold,
                    TotalPaidOrders = o.TotalPaidOrders,
                    TotalUnpaidOrders = o.TotalUnpaidOrders,
                    TotalCustomers = o.TotalCustomers,
                    StatisticType = o.StatisticType
                })
                .ToListAsync();

            return result;
        }

        public async Task<bool> DeleteOrderStatisticsAsync(int id)
        {
            try
            {
                var OrderStatisticsId = await _context.OrderStatistics.FindAsync(id);
                if (OrderStatisticsId == null)
                { return false; }
                _context.Remove(OrderStatisticsId);
                var deleteResult = await _context.SaveChangesAsync();
                return deleteResult > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }

        }
        // Thống kê theo ngày 
        public async Task<OrderStatisticsDto?> GetDayOrderStatisticsAsync(DateTime? date = null)
        {
            var targetDate = date?.Date ?? DateTime.Today;

            var statistic = await _context.OrderStatistics
                .Where(s => s.StatisticType == "Day" && s.StatisticDate.Date == targetDate)
                .Select(s => new OrderStatisticsDto
                {
                    OrderStatisticId = s.OrderStatisticId,
                    StatisticDate = s.StatisticDate.ToString("yyyy-MM-dd"),
                    TotalRevenue = s.TotalRevenue,
                    TotalOrders = s.TotalOrders,
                    TotalProductsSold = s.TotalProductsSold,
                    TotalPaidOrders = s.TotalPaidOrders,
                    TotalUnpaidOrders = s.TotalUnpaidOrders,
                    TotalCustomers = s.TotalCustomers,
                    StatisticType = s.StatisticType
                })
                .FirstOrDefaultAsync();

            return statistic;
        }

        // Thống kê theo tháng
        public async Task<OrderStatisticsDto?> GetMonthOrderStatisticsAsync(int? month = null, int? year = null)
        {
            var targetMonth = month ?? DateTime.Now.Month;
            var targetYear = year ?? DateTime.Now.Year;

            var statistic = await _context.OrderStatistics
                .Where(s => s.StatisticType == "Month" &&
                            s.StatisticDate.Month == targetMonth &&
                            s.StatisticDate.Year == targetYear)
                .Select(s => new OrderStatisticsDto
                {
                    OrderStatisticId = s.OrderStatisticId,
                    StatisticDate = s.StatisticDate.ToString("yyyy-MM"),
                    TotalRevenue = s.TotalRevenue,
                    TotalOrders = s.TotalOrders,
                    TotalProductsSold = s.TotalProductsSold,
                    TotalPaidOrders = s.TotalPaidOrders,
                    TotalUnpaidOrders = s.TotalUnpaidOrders,
                    TotalCustomers = s.TotalCustomers,
                    StatisticType = s.StatisticType
                })
                .FirstOrDefaultAsync();

            return statistic;
        }

        // Thống kê theo năm 
        public async Task<OrderStatisticsDto?> GetYearOrderStatisticsAsync(int? year = null)
        {
            var targetYear = year ?? DateTime.Now.Year;

            var statistic = await _context.OrderStatistics
                .Where(s => s.StatisticType == "Year" &&
                            s.StatisticDate.Year == targetYear)
                .Select(s => new OrderStatisticsDto
                {
                    OrderStatisticId = s.OrderStatisticId,
                    StatisticDate = s.StatisticDate.ToString("yyyy"),
                    TotalRevenue = s.TotalRevenue,
                    TotalOrders = s.TotalOrders,
                    TotalProductsSold = s.TotalProductsSold,
                    TotalPaidOrders = s.TotalPaidOrders,
                    TotalUnpaidOrders = s.TotalUnpaidOrders,
                    TotalCustomers = s.TotalCustomers,
                    StatisticType = s.StatisticType
                })
                .FirstOrDefaultAsync();

            return statistic;
        }







    }
}
