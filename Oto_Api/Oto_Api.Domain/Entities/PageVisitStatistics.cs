using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class PageVisitStatistics
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageVisitStatisticId { get; set; }

        // Ngày thống kê
        [Required]
        public DateTime StatisticDate { get; set; }

        // Tổng lượt truy cập
        public int? TotalVisits { get; set; }

        // Tổng lượt truy cập duy nhất (Unique Visitors)

        public int? UniqueVisits { get; set; }

        // Tổng lượt truy cập từ các chiến dịch (ví dụ: quảng cáo, email)
        public int? CampaignVisits { get; set; }

        // Loại thống kê: Ngày, Tháng, Năm
        [Required]
        public string? StatisticType { get; set; } // "Day", "Month", "Year"

        public string? StatisticDateFormat => StatisticDate.ToString(StatisticType == "Day" ? "yyyy-MM-dd" : StatisticType == "Month" ? "yyyy-MM" : "yyyy");
    }
}
