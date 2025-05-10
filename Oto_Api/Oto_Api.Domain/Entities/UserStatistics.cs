using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class UserStatistics
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserStatisticId { get; set; }

        // Ngày thống kê
        [Required]
        public DateTime StatisticDate { get; set; }

        // Tổng số khách hàng
        public int? TotalCustomers { get; set; }

        // Tổng số khách hàng mới
        public int? TotalNewCustomers { get; set; }

        // Tổng số khách hàng quay lại (active)
        public int? TotalReturningCustomers { get; set; }

        // Tỷ lệ khách hàng mua hàng
        public decimal? PurchaseRate => (TotalCustomers > 0) ? (decimal)TotalNewCustomers / TotalCustomers * 100 : 0;

        // Loại thống kê: Ngày, Tháng, Năm
        public string? StatisticType { get; set; } // "Day", "Month", "Year"

        public string? StatisticDateFormat => StatisticDate.ToString(StatisticType == "Day" ? "yyyy-MM-dd" : StatisticType == "Month" ? "yyyy-MM" : "yyyy");
    }
}
