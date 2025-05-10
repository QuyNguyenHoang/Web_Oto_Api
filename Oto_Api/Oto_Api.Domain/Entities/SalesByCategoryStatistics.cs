using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class SalesByCategoryStatistics
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesByCategoryStatisticId { get; set; }

        // Mã danh mục sản phẩm
        [Required]
        public int CategoryId { get; set; }

        // Tên danh mục

        public string? CategoryName { get; set; }

        // Tổng doanh thu từ sản phẩm trong danh mục này
        public decimal? TotalRevenue { get; set; }

        // Tổng số sản phẩm bán ra trong danh mục này

        public int? TotalProductsSold { get; set; }

        // Ngày thống kê
        public DateTime StatisticDate { get; set; }

        // Loại thống kê: Ngày, Tháng, Năm
        public string? StatisticType { get; set; }

        public string? StatisticDateFormat => StatisticDate.ToString(StatisticType == "Day" ? "yyyy-MM-dd" : StatisticType == "Month" ? "yyyy-MM" : "yyyy");
    }
}
