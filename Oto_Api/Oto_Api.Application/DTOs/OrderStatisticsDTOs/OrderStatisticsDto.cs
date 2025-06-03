using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.OrderStatisticsDTOs
{
    public class OrderStatisticsDto
    {
        public int OrderStatisticId { get; set; }

        public string StatisticDate { get; set; } = string.Empty; // Dạng chuỗi yyyy-MM-dd / yyyy-MM / yyyy

        public decimal TotalRevenue { get; set; }

        public int TotalOrders { get; set; }

        public int TotalProductsSold { get; set; }

        public int TotalPaidOrders { get; set; }

        public int TotalUnpaidOrders { get; set; }

        public int TotalCustomers { get; set; }

        public string StatisticType { get; set; } = string.Empty; // Day / Month / Year
    }
}
