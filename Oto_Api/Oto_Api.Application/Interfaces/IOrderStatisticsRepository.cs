using Oto_Api.Application.DTOs.OrderStatisticsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface IOrderStatisticsRepository
    {
        Task<bool> CreateOrUpdateOrderStatisticsAsync(DateTime date);
        Task<bool> CreateOrUpdateOrderStatisticsByMonthAsync(int year, int month);
        Task<bool> CreateOrUpdateOrderStatisticsByYearAsync(int year);
        Task<List<OrderStatisticsDto>> GetAllOrderStatisticsAsync();
        Task<List<OrderStatisticsDto>> SearchOrderStatisticsByDateAsync(DateTime? searchDate, int pageNumber, int pageSize);
        Task<int> CountOrderStatisticsAsync(DateTime? searchDate);
        Task<bool> DeleteOrderStatisticsAsync(int id);
        Task<OrderStatisticsDto?> GetDayOrderStatisticsAsync(DateTime? date = null);
        Task<OrderStatisticsDto?> GetMonthOrderStatisticsAsync(int? month = null, int? year = null);
        Task<OrderStatisticsDto?> GetYearOrderStatisticsAsync(int? year = null);
    }
}
