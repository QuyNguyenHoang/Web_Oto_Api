using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class OrderDetails
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? Discount { get; set; }

        public decimal? TotalPrice => Price * Quantity * (1 - (Discount ?? 0) / 100);


        public int? VoucherId { get; set; }

        // Mối quan hệ với Order
        [ForeignKey("OrderId")]
        public virtual Orders? Order { get; set; }

        // Mối quan hệ với Product
        [ForeignKey("ProductId")]
        public virtual Products? Product { get; set; }

        // Mối quan hệ với Voucher
        [ForeignKey("VoucherId")]
        public virtual Vouchers? Voucher { get; set; }
    }
}
