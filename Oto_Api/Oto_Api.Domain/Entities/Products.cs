using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Oto_Api.Domain.Entities
{
    public class Products
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string? ProductName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsAvailable { get; set; } = true;

        // Khóa ngoại đến Category
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual Categories? Categories { get; set; }

        public virtual ICollection<Stocks>? Stocks { get; set; }
        public virtual ICollection<Pictures>? Pictures { get; set; }
        public virtual ICollection<Prices>? Prices { get; set; }
    }
}
