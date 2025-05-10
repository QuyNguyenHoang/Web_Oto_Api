using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class Stocks
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }

        public int? Quantity { get; set; }

        [StringLength(255)]
        public string? WarehouseLocation { get; set; }

        // KHoa ngoai den Product
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        public virtual Products? Product { get; set; }
    }
}
