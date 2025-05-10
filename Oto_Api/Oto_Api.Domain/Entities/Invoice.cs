using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class Invoice
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string? InvoiceNumber { get; set; }

        [Required]
        public int OrderId { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required]
        public decimal? TotalAmount { get; set; }

        [StringLength(500)]
        public string? PaymentMethod { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime? PaymentDate { get; set; }

        // Mối quan hệ với Order
        [ForeignKey("OrderId")]
        public virtual Orders? Order { get; set; }
        // Mối quan hệ với User
        [ForeignKey("Id")]
        public virtual User? User { get; set; }

        [StringLength(255)]
        public string? BillingAddress { get; set; }
        public virtual ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
    }
}
