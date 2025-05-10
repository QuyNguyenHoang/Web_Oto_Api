using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class ProductReviews
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string? Id { get; set; }


        [Range(1, 5)]
        public int? Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Mối quan hệ với sản phẩm
        [ForeignKey("ProductId")]
        public virtual Products? Product { get; set; }

        // Mối quan hệ với người dùng
        [ForeignKey("Id")]
        public virtual User? User { get; set; }
    }
}
