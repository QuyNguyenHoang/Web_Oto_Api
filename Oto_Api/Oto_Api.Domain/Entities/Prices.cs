using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oto_Api.Domain.Entities
{
    public class Prices
    {
        [Key]
        public int PriceId { get; set; }


        [Required]
        public decimal PriceIn { get; set; } // Giá nhập vào

        [Required]
        public decimal PriceSale { get; set; } // Giá bán

        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        // FK tới Product
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Products? Product { get; set; }
    }
}
