using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class InvoiceDetail
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceDetailId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal? UnitPrice { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Total => UnitPrice * Quantity * (1 - Discount / 100);

        // Mối quan hệ với Invoice
        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }

        // Mối quan hệ với Product
        [ForeignKey("ProductId")]
        public virtual Products? Product { get; set; }
    }
}
