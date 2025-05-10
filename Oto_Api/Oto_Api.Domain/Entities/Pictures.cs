using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class Pictures
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PictureId { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        public virtual Products? Product { get; set; }
    }
}
