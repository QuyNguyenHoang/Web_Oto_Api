using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class TransactionHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        public string? Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int? PaymentMethodId { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        // Mối quan hệ với Order
        [ForeignKey("OrderId")]
        public virtual Orders Order { get; set; }

        // Mối quan hệ với User
        [ForeignKey("Id")]
        public virtual User? User { get; set; }

        // Mối quan hệ với PaymentMethod
        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
