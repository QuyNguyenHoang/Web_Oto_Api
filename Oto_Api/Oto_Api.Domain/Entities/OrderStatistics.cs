using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class OrderStatistics
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderStatisticId { get; set; }

        // Ngày thống kê
        [Required]
        public DateTime StatisticDate { get; set; }

        // Tổng doanh thu của tất cả đơn hàng
        [Required]
        public decimal TotalRevenue { get; set; }

        // Tổng số đơn hàng
        [Required]
        public int TotalOrders { get; set; }

        // Tổng số sản phẩm bán ra
        [Required]
        public int TotalProductsSold { get; set; }

        // Tổng số đơn hàng đã thanh toán
        [Required]
        public int TotalPaidOrders { get; set; }

        // Tổng số đơn hàng chưa thanh toán
        [Required]
        public int TotalUnpaidOrders { get; set; }

        // Tổng số khách hàng đã mua hàng
        [Required]
        public int TotalCustomers { get; set; }

        // Loại thống kê: Ngày, Tháng, Năm
        [Required]
        public string? StatisticType { get; set; }

        public string StatisticDateFormat => StatisticDate.ToString(StatisticType == "Day" ? "yyyy-MM-dd" : StatisticType == "Month" ? "yyyy-MM" : "yyyy");
    }
}
