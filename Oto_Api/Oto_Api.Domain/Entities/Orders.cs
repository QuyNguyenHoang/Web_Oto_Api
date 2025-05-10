using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class Orders
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public string? Id { get; set; }

        public decimal? TotalAmount { get; set; }
        public int? VoucherId { get; set; }
        public int? ShipperId { get; set; }
        public string? Status { get; set; } = "Pending";

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime? ShippingDate { get; set; }

        public string? ShippingAddress { get; set; }

        // Mối quan hệ với User
        [ForeignKey("Id")]
        public virtual User? User { get; set; }
        // Mối quan hệ với Shipper
        [ForeignKey("ShipperId")]
        public virtual Shipper? Shipper { get; set; }
        //Mối quan hệ Vocher
        [ForeignKey("VoucherId")]
        public virtual Vouchers? Voucher { get; set; }
        //Hoa don
        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }
        // Mối quan hệ với các OrderDetails 
        public virtual ICollection<OrderDetails>? OrderDetails { get; set; }
    }
}
