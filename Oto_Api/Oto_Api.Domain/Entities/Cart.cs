using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class Cart
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        [Required]
        public string? Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public decimal? TotalPrice => (Price * Quantity) - (Discount ?? 0);

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public decimal? Discount { get; set; }

        public string? SessionId { get; set; }

        public string? ShippingMethod { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }

        // Mối quan hệ với Product
        [ForeignKey("ProductId")]
        public virtual Products? Product { get; set; }

        // Mối quan hệ với User
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
